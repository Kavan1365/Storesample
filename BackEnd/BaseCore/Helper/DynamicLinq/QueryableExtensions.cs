using BaseCore.Api;
using BaseCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace BaseCore.Helper.DynamicLinq
{
    public static class QueryableExtensions
    {
        public static /*async Task<*/ApiDataSourceResult/*>*/ ToDataSourceResultAsync<T, TModel>(this IQueryable<T> query, DataSourceRequest request)
        {
            var filter = DynamicLinq.Filter<TModel>.GetExpression(request.Filter);

            if (filter != null && !string.IsNullOrEmpty(filter))
            {
                query = query.Where(filter);
            }

            var sort = DynamicLinq.Sort.GetSortExpression(request.sort);

            if (sort != null)
            {
                query = query.OrderBy(sort);
            }




            var total = query.Count();

            if (request.skip + request.take > 0)
            {
                query = query.Skip(request.skip).Take(request.take);
            }
            var aggregate = Aggregate(query, request.Aggregate, total);

            var result = new ApiDataSourceResult
            {
                Data = query.ToList(),
                Total = total,
                Aggregates = aggregate
            };

            return result;
        }



        public static ApiDataSourceResult ToDataSourceResultTree<T>(this IQueryable<T> queryable, int take, int skip,
            IEnumerable<SortTree> sort, FilterTreeList filter, IEnumerable<Aggregator> aggregates, IEnumerable<Group> group)
        {
            queryable = Filter(queryable, filter);

            var total = queryable.Count();
            var aggregate = Aggregate(queryable, aggregates, total);

            var result = new ApiDataSourceResult
            {
                Total = total,
                Aggregates = aggregate
            };

            var pagedQuery = queryable.Sort(sort, group).Page(take, skip);

            if (group != null && group.Any())
            {
                result.Group = GroupBy(queryable, pagedQuery, group.First());
            }
            else
            {
                result.Data = pagedQuery.ToList();
            }
            return result;
        }

        private static IEnumerable<GroupResult> GroupBy<T>(IQueryable<T> queryable, IQueryable<T> pagedQuery, Group group)
        {
            var result = new List<GroupResult>();


            var groupExpression = (Func<T, object>)DynamicExpressionParser.ParseLambda(typeof(T), typeof(object), group.Field).Compile();

            var pagedQueryGroups = pagedQuery.GroupBy(groupExpression).ToList();
            var queryGroups = queryable.GroupBy(groupExpression);

            foreach (var item in pagedQueryGroups)
            {
                var groupItems = queryGroups.First(g => item.Key.Equals(g.Key));

                result.Add(new GroupResult
                {
                    Value = item.Key,
                    HasSubgroups = false,
                    Count = groupItems.Count(),
                    Items = item,
                    Field = group.Field.Replace(".", ""),
                    Aggregates = groupItems.AsQueryable().Aggregate(group.Aggregates, groupItems.Count())
                });
            }

            return result;
        }
        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, FilterTreeList filter)
        {
            if (filter != null && filter.Logic != null)
            {

                // Collect a flat list of all filters
                var filters = filter.All();

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).Distinct().ToArray();

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                string predicate = filter.ToExpression(filters);
                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);




            }

            return queryable;
        }

        private static object Aggregate<T>(this IQueryable<T> queryable, IEnumerable<Aggregator> aggregates, int count)
        {
            if (aggregates == null || !aggregates.Any() || count == 0)
                return null;

            var objProps = new Dictionary<DynamicProperty, object>();
            var groups = aggregates.GroupBy(g => g.Field);
            Type type = null;
            foreach (var group in groups)
            {
                var fieldProps = new Dictionary<DynamicProperty, object>();
                foreach (var aggregate in group)
                {
                    var prop = typeof(T).GetProperty(aggregate.Field);
                    var param = Expression.Parameter(typeof(T), "s");
                    var selector = aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                        ? Expression.Lambda(Expression.NotEqual(Expression.MakeMemberAccess(param, prop), Expression.Constant(null, prop.PropertyType)), param)
                        : Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);
                    var mi = aggregate.MethodInfo(typeof(T));
                    if (mi == null)
                        continue;

                    var val = queryable.Provider.Execute(Expression.Call(null, mi,
                        aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                            ? new[] { queryable.Expression }
                            : new[] { queryable.Expression, Expression.Quote(selector) }));

                    fieldProps.Add(new DynamicProperty(aggregate.Aggregate, typeof(object)), val);
                }
                type = MyDynamicExpression.CreateClass(fieldProps.Keys);
                var fieldObj = Activator.CreateInstance(type);
                foreach (var p in fieldProps.Keys)
                    type.GetProperty(p.Name).SetValue(fieldObj, fieldProps[p], null);
                objProps.Add(new DynamicProperty(group.Key, fieldObj.GetType()), fieldObj);
            }

            type = MyDynamicExpression.CreateClass(objProps.Keys);

            var obj = Activator.CreateInstance(type);

            foreach (var p in objProps.Keys)
            {
                type.GetProperty(p.Name).SetValue(obj, objProps[p], null);
            }

            return obj;

        }
        private static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<SortTree> sort, IEnumerable<Group> group)
        {
            var sort1 = new List<SortTree>();

            if (group != null && group.Any())
                sort1.AddRange(group.Reverse());

            if (sort != null && sort.Any())
                sort1.AddRange(sort);

            if (sort1.Any())
            {
                var ordering = String.Join(",", sort1.Select(s => s.ToExpression()));
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private static IQueryable<T> Page<T>(this IQueryable<T> queryable, int take, int skip)
        {
            if (take > 0)
                return queryable.Skip(skip).Take(take);
            else
                return queryable;
        }







    }
}
