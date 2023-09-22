using AutoMapper;
using BaseCore.Api;
using BaseCore.Configuration.AutoMapper;
using BaseCore.Helper.DynamicLinq;
using BaseCore.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Dapper;
using System.Threading.Tasks;
using BaseCore.Dapper;
using System.Data;
using GroupResult = BaseCore.Helper.DynamicLinq.GroupResult;

namespace BaseCore.Configuration
{
    public static class KendoGrid
    {
        public static ApiDataSourceResult ToDataSource<TModel>(this IQueryable<TModel> query, DataSourceRequest request)
        {

            var filter = Filter<TModel>.GetExpression(request.Filter);
            if (filter != null)
            {
                query = query.Where(filter);
            }


            var total = query.Count();

            var aggregate = Aggregate(query, request.Aggregate, total);
            var result = new ApiDataSourceResult
            {
                Total = total,
                Aggregates = aggregate

            };
            if (request.Group != null)
            {
                foreach (var group in request.Group)
                {
                    var property = typeof(TModel).GetProperty(group.Field);
                    if (property.IsDefined(typeof(NestedFieldAttribute), false))
                    {
                        group.Field = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
                    }

                }
            }


            var sort = Sort.GetSortExpression(request.sort);

            if (sort != null)
            {
                query = query.OrderBy(sort);
            }


            if (request.skip + request.take > 0)
            {
                query = query.Skip(request.skip).Take(request.take);
            }


            if (request.Group != null && request.Group.Any())
            {
                result.Group = GroupBy(query, query, request.Group.First());
            }
            else
            {
                result.Data = query.ToList();
            }

            return result;
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


    }


    public class JsonDataSourceResult : ActionResult
    {
        private object _source { get; set; }


        public JsonDataSourceResult(object source)
        {
            _source = source;
        }



        public override void ExecuteResult(ActionContext context)
        {

            var source = _source;
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                //NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            });

            context.HttpContext.Response.ContentType = "application/json";

            using (var sw = new StringWriter())
            {
                jsonSerializer.Serialize(sw, source);

                context.HttpContext.Response.WriteAsync(sw.ToString());
            }


            base.ExecuteResult(context);
        }

    }

    public class DataSourceResult<TDestination> : ActionResult
    {
        private IQueryable<TDestination> _source { get; set; }
        private DataSourceRequest _request { get; set; }
        private Action<IEnumerable> _prepareItems { get; set; }



        public DataSourceResult(IQueryable<TDestination> query, DataSourceRequest request,
            Action<IEnumerable> prepareItems = null)
        {
            _source = query;
            _request = request;
            _prepareItems = prepareItems;
        }


        public override async Task ExecuteResultAsync(ActionContext context)
        {

            var source = _source.ToDataSource<TDestination>(_request);

            if (source.Data != null)
            {
                try
                {
                    var data = (source.Data);
                    _prepareItems?.Invoke(data);
                    source.Data = data;

                }
                catch 
                {

                    var data = source.Data;
                    //_prepareItems?.Invoke(data);
                    source.Data = data;

                }

            }
            else if (source.Group != null)
            {

                foreach (var group in source.Group)
                {
                    var data = (group.Items);
                    _prepareItems?.Invoke(data);
                    group.Items = data;
                }
            }




            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            });

            context.HttpContext.Response.ContentType = "application/json";

            using (var sw = new StringWriter())
            {
                jsonSerializer.Serialize(sw, source);

                await context.HttpContext.Response.WriteAsync(sw.ToString(), cancellationToken: CancellationToken.None);
            }

            await base.ExecuteResultAsync(context);
        }

    }

    public class DataSourceResultTree<TDestination> : ActionResult
    {
        private readonly IMapper _mapper;
        private IQueryable<TDestination> _source { get; set; }
        private bool _isHierarchical { get; set; }
        private DataSourceRequestTree _request { get; set; }
        private Action<IEnumerable<TDestination>> _prepareItems { get; set; }

        private ApiDataSourceResult _result { get; set; }

        private int? _rootId;


        public DataSourceResultTree(IQueryable<TDestination> source, DataSourceRequestTree request, IMapper mapper,
            bool isHierarchical = false, int? rootId = null, Action<IEnumerable<TDestination>> prepareItems = null)
        {
            _source = source;
            _isHierarchical = isHierarchical;
            _mapper = mapper;
            _request = request;
            _rootId = rootId;
            _prepareItems = prepareItems;
        }

        public override void ExecuteResult(ActionContext context)
        {
            var request = _request;
            if ((request.Sort == null || request.Sort.Count() == 0))
            {
                if (_source.Expression.Type != typeof(IOrderedQueryable<TDestination>))
                    request.Sort = new List<SortTree>() { new SortTree { Field = "Id", Dir = "desc" } };
            }

            else
            {
                foreach (var sort in request.Sort.Distinct().Where(x => x.Field != null))
                {
                    var property = typeof(TDestination).GetProperty(sort.Field);
                    if (property.IsDefined(typeof(NestedFieldAttribute), false))
                    {
                        var chain = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
                        sort.Field = chain;
                    }
                }

            }

            if (request.Filter != null)
                foreach (var filter in request.Filter?.All().Distinct().Where(x => x.Field != null))
                {
                    var property = typeof(TDestination).GetProperty(filter.Field);
                    if (property == null)
                        continue;

                    if (property.PropertyType.IsEnum)
                        filter.Value = Enum.ToObject(property.PropertyType, filter.Value);

                    if (property.IsDefined(typeof(NestedFieldAttribute), false))
                    {
                        var chain = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
                        if (chain.Contains(','))
                        {
                            //Search value in more than one fields.
                            var temp = chain.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            filter.Field = null;
                            filter.Logic = "or";
                            filter.Filters = new List<FilterTreeList>();
                            foreach (var item in temp)
                            {
                                filter.Filters.Add(new FilterTreeList
                                {
                                    Field = item,
                                    Operator = filter.Operator,
                                    Value = filter.Value,
                                });
                            }

                            filter.Value = null;
                            filter.Operator = null;
                        }
                        else
                        {
                            filter.Field = chain;
                        }

                    }

                    if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        var date = Convert.ToDateTime(filter.Value).Date;
                        filter.Field = $"EF.Functions.DateDiffDay({filter.Field},{date})";
                        filter.Value = 0;
                    }
                }

            if (request.Group != null)
            {
                foreach (var group in request.Group)
                {
                    var property = typeof(TDestination).GetProperty(group.Field);

                    if (property?.IsDefined(typeof(NestedFieldAttribute), false) ?? false)
                        group.Field = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
                }
            }

            if (_isHierarchical)
            {
                var filters = new List<FilterTreeList>();
                if (request.Filter == null)
                {
                    request.Filter = new FilterTreeList { Logic = "and" };
                }
                else
                {
                    foreach (var item in request.Filter.Filters)
                    {
                        filters.Add(new FilterTreeList { Field = item.Field, Operator = item.Operator, Value = item.Value });

                    }
                }



                if (!request.Id.HasValue && _rootId.HasValue)
                    filters.Add(new FilterTreeList { Field = "Id", Operator = "eq", Value = _rootId.Value });
                else
                    filters.Add(new FilterTreeList { Field = "ParentId", Operator = request.Id.HasValue ? "eq" : "isnull", Value = request.Id });

                request.Filter.Filters = filters;

            }

            var source = _source.ToDataSourceResultTree(request.Take, request.Skip, request.Sort, request.Filter,
                request.Aggregate, request.Group);

            if (source.Data != null)
            {
                var data = _mapper.Map<List<TDestination>>(source.Data);
                _prepareItems?.Invoke(data);
                source.Data = data;

            }
            else if (source.Group != null)
            {
                foreach (var group in source.Group)
                {
                    var data = _mapper.Map<List<TDestination>>(group.Items);
                    _prepareItems?.Invoke(data);
                    group.Items = data;
                }

                foreach (var group in source.Group)
                {
                    var data = _mapper.Map<List<TDestination>>(group.Items);
                    _prepareItems?.Invoke(data);
                    group.Items = data;
                }
            }


            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                //NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            });

            context.HttpContext.Response.ContentType = "application/json";

            using (var sw = new StringWriter())
            {
                jsonSerializer.Serialize(sw, source);

                context.HttpContext.Response.WriteAsync(sw.ToString());
            }


            base.ExecuteResult(context);
        }

    }



    public class DataSourceResultDapper<TDestination> : ActionResult
    {
        private string StoredProcedures { get; set; }
        private string Condition { get; set; }

        private readonly IDapper _dapper;
        private DataSourceRequest _request { get; set; }
        private Action<IEnumerable> _prepareItems { get; set; }



        public DataSourceResultDapper(string storedProcedures, string condition, IDapper dapper, DataSourceRequest request,
            Action<IEnumerable> prepareItems = null)
        {
            Condition = condition;
            _dapper = dapper;
            StoredProcedures = storedProcedures;
            _request = request;
            _prepareItems = prepareItems;
        }


        public override async Task ExecuteResultAsync(ActionContext context)
        {

            var getfilters = "";


            var filter = Filter<TDestination>.GetExpressionDapper(_request.Filter);
            if (filter != null)
            {
                getfilters = $"'where {filter} and {Condition}'";
            }
            else
            {
                getfilters = $"'where {Condition}'";

            }

            var sort = Sort.GetSortExpressionDapper<TDestination>(_request.sort);
            var getsort = "";
            if (sort != null)
            {
                getsort = "ORDER BY " + sort;
            }
            else
            {
                getsort = "ORDER BY Id DESC ";
            }


            var sqlrawFilter = $"execute  {StoredProcedures} @take = N'{_request.take}', @skip = N'{_request.skip}', @sort = '{getsort}', @condition = {getfilters} ";
            var sqlrawFiltercont = $"execute  {StoredProcedures}Count  @condition = {getfilters}";

            var source = await Task.FromResult(_dapper.GetAll<TDestination>($"{sqlrawFilter}", null, commandType: CommandType.Text));
            var total = await Task.FromResult(_dapper.Get<int>($"{sqlrawFiltercont}", null, commandType: CommandType.Text)); ;

            //var aggregate = Aggregate(query, request.Aggregate, total);
            var result = new ApiDataSourceResult
            {
                Total = total,
                Data = source.ToList(),
                Aggregates = null,
                Errors = null,
                Group = null
                // Aggregates = aggregate

            };
            //if (request.Group != null)
            //{
            //    foreach (var group in request.Group)
            //    {
            //        var property = typeof(TModel).GetProperty(group.Field);
            //        if (property.IsDefined(typeof(NestedFieldAttribute), false))
            //        {
            //            group.Field = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
            //        }

            //    }
            //}








            //if (request.Group != null && request.Group.Any())
            //{
            //    result.Group = GroupBy(query, query, request.Group.First());
            //}
            //else
            //{

            //}





            //if (result.Data != null)
            //{
            //    try
            //    {
            //        var data = (result.Data);
            //        _prepareItems?.Invoke(data);
            //        result.Data = data;

            //    }
            //    catch (Exception e)
            //    {

            //        var data = result.Data;
            //        //_prepareItems?.Invoke(data);
            //        result.Data = data;

            //    }

            //}
            //else if (source.Group != null)
            //{

            //    foreach (var group in source.Group)
            //    {
            //        var data = (group.Items);
            //        _prepareItems?.Invoke(data);
            //        group.Items = data;
            //    }
            //}




            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            });

            context.HttpContext.Response.ContentType = "application/json";

            using (var sw = new StringWriter())
            {
                jsonSerializer.Serialize(sw, result);

                await context.HttpContext.Response.WriteAsync(sw.ToString(), cancellationToken: CancellationToken.None);
            }

            await base.ExecuteResultAsync(context);
        }

    }


}
