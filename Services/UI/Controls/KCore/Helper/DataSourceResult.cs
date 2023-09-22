using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Services.UI.Controls.KCore.Helper
{
    //public class DataSourceResult<TSource, TDestination> : ActionResult
    //{
    //    private IMapper _mapper;
    //    private IQueryable<TSource> _source { get; set; }
    //    private bool _isHierarchical { get; set; }

    //    private Action<IEnumerable<TDestination>> _prepareItems { get; set; }

    //    private MvcDataSourceResult _result { get; set; }

    //    private int? _rootId;

    //    public DataSourceResult(IQueryable<TSource> source,
    //        bool isHierarchical = false, int? rootId = null, Action<IEnumerable<TDestination>> prepareItems = null)
    //    {
    //        _source = source;
    //        _isHierarchical = isHierarchical;
    //        _rootId = rootId;
    //        _prepareItems = prepareItems;
    //    }

    //    public override void ExecuteResult(ActionContext context)
    //    {
    //        var queryString = context.HttpContext.Request.Query;

    //        //var queryString = context.RequestContext.HttpContext.Request.QueryString.Get(null);
    //        var request = JsonConvert.DeserializeObject<MvcDataSourceRequest>("", new JsonSerializerSettings
    //        {
    //            DateTimeZoneHandling = DateTimeZoneHandling.Local
    //        });

    //        //The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'
    //        if ((request.Sort == null || request.Sort.Count() == 0))
    //        {
    //            if (_source.Expression.Type != typeof(IOrderedQueryable<TSource>))
    //                request.Sort = new List<Sort>() {new Sort {Field = "Id", Dir = "desc"}};
    //        }

    //        else
    //        {
    //            foreach (var sort in request.Sort.Distinct().Where(x => x.Field != null))
    //            {
    //                var property = typeof(TDestination).GetProperty(sort.Field);
    //                if (property.IsDefined(typeof(NestedFieldAttribute), false))
    //                {
    //                    var chain = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
    //                    sort.Field = chain;
    //                }
    //            }

    //        }

    //        if (request.Filter != null)
    //            foreach (var filter in request.Filter?.All().Distinct().Where(x => x.Field != null))
    //            {
    //                var property = typeof(TDestination).GetProperty(filter.Field);
    //                if (property == null)
    //                    continue;

    //                if (property.PropertyType.IsEnum)
    //                    filter.Value = Enum.ToObject(property.PropertyType, filter.Value);

    //                if (property.IsDefined(typeof(NestedFieldAttribute), false))
    //                {
    //                    var chain = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
    //                    if (chain.Contains(','))
    //                    {
    //                        //Search value in more than one fields.
    //                        var temp = chain.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
    //                        filter.Field = null;
    //                        filter.Logic = "or";
    //                        filter.Filters = new List<Filter>();
    //                        foreach (var item in temp)
    //                        {
    //                            filter.Filters.Add(new Filter
    //                            {
    //                                Field = item,
    //                                Operator = filter.Operator,
    //                                Value = filter.Value,
    //                            });
    //                        }

    //                        filter.Value = null;
    //                        filter.Operator = null;
    //                    }
    //                    else
    //                    {
    //                        filter.Field = chain;
    //                    }

    //                }

    //                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
    //                {
    //                    filter.Value = ((DateTime) filter.Value).Date;
    //                    filter.Field = $"DbFunctions.TruncateTime({filter.Field})";
    //                }
    //            }

    //        if (request.Group != null)
    //        {
    //            foreach (var group in request.Group)
    //            {
    //                var property = typeof(TDestination).GetProperty(group.Field);

    //                if (property?.IsDefined(typeof(NestedFieldAttribute), false) ?? false)
    //                    group.Field = property.GetCustomAttribute<NestedFieldAttribute>().Chain;
    //            }
    //        }

    //        if (_isHierarchical)
    //        {
    //            var filters = new List<Filter>();
    //            if (request.Filter == null)
    //            {
    //                request.Filter = new Filter {Logic = "and"};
    //            }
    //            else
    //            {
    //                foreach (var item in request.Filter.Filters)
    //                {
    //                    filters.Add(new Filter {Field = item.Field, Operator = item.Operator, Value = item.Value});

    //                }
    //            }



    //            if (!request.Id.HasValue && _rootId.HasValue)
    //                filters.Add(new Filter {Field = "Id", Operator = "eq", Value = _rootId.Value});
    //            else
    //                filters.Add(new Filter
    //                    {Field = "ParentId", Operator = request.Id.HasValue ? "eq" : "isnull", Value = request.Id});

    //            request.Filter.Filters = filters;

    //        }

    //        var source = _source.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter,
    //            request.Aggregate, request.Group);

    //        if (source.Data != null)
    //        {
    //            var data = _mapper.Map<List<TDestination>>(source.Data);
    //            _prepareItems?.Invoke(data);
    //            source.Data = data;

    //        }
    //        else if (source.Group != null)
    //        {
    //            foreach (var group in source.Group)
    //            {
    //                var data = _mapper.Map<List<TDestination>>(group.Items);
    //                _prepareItems?.Invoke(data);
    //                group.Items = data;
    //            }
    //        }


    //        var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
    //        {
    //            //NullValueHandling = NullValueHandling.Ignore,
    //            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
    //        });

    //        context.HttpContext.Response.ContentType = "application/json";
    //        using (var sw = new StringWriter())
    //        {
    //            jsonSerializer.Serialize(sw, source);
    //            sw.ToString();

    //            // context.HttpContext.Response.Write(sw.ToString());
    //        }


    //        base.ExecuteResult(context);
    //    }
       
    //}

}
