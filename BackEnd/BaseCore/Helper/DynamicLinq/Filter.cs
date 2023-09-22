using BaseCore.Configuration.AutoMapper;
using BaseCore.Helper.Attribute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BaseCore.Helper.DynamicLinq
{


    internal class Filter<TModel>
    {
        private static string GetPropertyType(Type type, string field)
        {
            var info = type.GetProperty(field);

            if (info == null)
                return string.Empty;

            var nullableType = Nullable.GetUnderlyingType(info.PropertyType);
            return nullableType != null ? nullableType.Name.ToLower() + "?" : info.PropertyType.Name.ToLower();
        }

        private static string GetPropertyTypeNestedField(Type type, string field)
        {
            var info = type.GetProperty(field);

            if (info != null && info.IsDefined(typeof(NestedFieldAttribute), false))
            {
                var chain = info.GetCustomAttribute<NestedFieldAttribute>().Chain;
                return chain;
            }
            return field;
        }

        private static string GetPropertyTypeSigma(Type type, string field)
        {
            var info = type.GetProperty(field);

            if (info != null && info.IsDefined(typeof(SigmaAttribute), false))
            {
                var name = info.GetCustomAttribute<SigmaAttribute>().Name;
                return name;
            }
            return "t1";
        }
        const string PersiandatedRegex = @"^1[34][0-9][0-9](\/|-|)((1[0-2])|(0[1-9])|([1-9]))(\/|-|)(([12][0-9])|(3[01])|0[1-9]|([1-9]))$";

        public static bool IsPersianDate(string date)
        {
            return Regex.IsMatch(date, PersiandatedRegex);
        }
        private static DateTime GetDate(string value)
        {
            if (value.Contains("T"))
            {
                //"1392-08-06T19:30:00.000Z";
                value = value.Split('T')[0].Replace('-', '/');
            }

            DateTime date;
            if (IsPersianDate(value))
            {
                date = DNTPersianUtils.Core.PersianDateTimeUtils.ToGregorianDateTime(value).Value;

                return date;
            }
            else
            {
                var provider = CultureInfo.GetCultureInfo("en-US");

                date = DateTime.ParseExact(value, new[] { "yyyy/MM/dd", "yyyy-MM-dd", "yyyy/MM/ddTHH:mm:ss" }, provider, DateTimeStyles.None);
                return date;
            }
        }
        private static TimeSpan GetTime(string time)
        {
            var dateTime = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
            return dateTime.TimeOfDay;
        }
        private static string GetExpression(string field, string op, string param)
        {
            if (string.IsNullOrEmpty(field))
            {
                return "";
            }
            var dataType = GetPropertyType(typeof(TModel), field);


            field = GetPropertyTypeNestedField(typeof(TModel), field);

            if (string.IsNullOrWhiteSpace(dataType))
                return string.Empty;

            if (dataType == "string")
            {
                param = @"""" + param + @"""";
            }

            if (dataType == "datetime" || dataType == "datetime?" || dataType == "datetimeoffset" || dataType == "datetimeoffset?")
            {
                if (dataType == "datetime?" || dataType == "datetimeoffset?")
                    field += ".Value";

                var date = GetDate(param);

                var eq = $"({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day == {date.Day})";
                var gt = $"(({field}.Year > {date.Year}) || ({field}.Year == {date.Year} && {field}.Month > {date.Month} ) || ({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day > {date.Day}))";
                // var lt = $"(({field}.Year < {date.Year}) || ({field}.Year == {date.Year} && {field}.Month < {date.Month} ) || ({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day < {date.Day}))";

                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"({eq})";
                        break;

                    case "neq":
                        exDate = $"!({eq})";
                        break;

                    case "gte":
                        exDate = $"({eq} || {gt})";
                        break;

                    case "gt":
                        exDate = $"({gt})";
                        break;

                    case "lte":
                        exDate = $"{eq} || !({gt})";
                        break;

                    case "lt":
                        exDate = $"!({eq}) && !({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "timespan" || dataType == "timespan?")
            {
                if (dataType == "timespan?")
                    field += ".Value";

                var date = GetTime(param);

                var eq = $"({field}.{nameof(date.Hours)} == {date.Hours} && {field}.{nameof(date.Minutes)} == {date.Minutes} && {field}.{nameof(date.Seconds)} == {date.Seconds})";
                var gt = $"(({field}.{nameof(date.Hours)} > {date.Hours}) || ({field}.{nameof(date.Hours)} == {date.Hours} && {field}.{nameof(date.Minutes)}  > {date.Minutes} ) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)}  == {date.Minutes} && {field}.{nameof(date.Seconds)}  > {date.Seconds}))";
                // var lt = $"(({field}.{nameof(date.Hours)} < {date.Hours}) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)} < {date.Minutes} ) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)}  == {date.Minutes} && {field}.{nameof(date.Seconds)}  < {date.Seconds}))";


                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"{eq}";
                        break;

                    case "neq":
                        exDate = $"!({eq})";
                        break;

                    case "gte":
                        exDate = $"{eq} || {gt}";
                        break;

                    case "gt":
                        exDate = $"{gt}";
                        break;
                    case "lte":
                        exDate = $"{eq} || !({gt})";
                        break;

                    case "lt":
                        exDate = $"!({eq}) && !({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "boolean" || dataType == "boolean?")
            {
                if (param.ToLower() == Boolean.TrueString.ToLower())
                    param = Boolean.TrueString.ToLower();

                if (param.ToLower() == Boolean.FalseString.ToLower())
                    param = Boolean.FalseString.ToLower();
            }

            string exStr,
                caseMod = string.Empty;
            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "doesnotcontain":
                    exStr = string.Format("!{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;

                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;

                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;

                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;

                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;

                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }


        public static string GetExpression(FilterDescription filter)
        {
            if (filter == null)
                return null;
            if (filter.Filters != null && filter.Filters.Count() == 0)
                return null;

            if (filter.Filters == null || !filter.Filters.Any())
            {
                //if (string.IsNullOrWhiteSpace(filter.Value))
                //    return null;

                var exprList = GetExpression(filter.Field, filter.Operator, filter.Value);
                return exprList;
            }

            var logic = filter.Logic.ToLower() == "or" ? "||" : "&&";

            var expressions = new List<string>();

            foreach (var item in filter.Filters)
            {
                var expr = GetExpression(item);

                if (!string.IsNullOrWhiteSpace(expr))
                    expressions.Add(expr);
            }

            if (expressions.Count == 0)
                return null;

            return "(" + string.Join(" " + logic + " ", expressions) + ")";
        }


        private static string GetExpressionDapper(string field, string op, string param)
        {
            var sigma = "t1";
            var dataType = GetPropertyType(typeof(TModel), field);


            sigma = GetPropertyTypeSigma(typeof(TModel), field);

            if (string.IsNullOrWhiteSpace(dataType))
                return string.Empty;

            if (dataType == "string")
            {
                param = @"" + param + @"";
            }

            if (dataType == "datetime" || dataType == "datetime?" || dataType == "datetimeoffset" || dataType == "datetimeoffset?")
            {
                if (dataType == "datetime?" || dataType == "datetimeoffset?")
                    field += "";

                var date = GetDate(param);

                var eq = $"( ((SELECT YEAR({sigma}.{field}))  = {date.Year}) and ((SELECT Month({sigma}.{field})) =  {date.Month})  and ((SELECT Day({sigma}.{field})) = {date.Day}) )";
                var gt = $"((((SELECT YEAR({sigma}.{field})) > {date.Year}) or (((SELECT YEAR({sigma}.{field})) = {date.Year} and (SELECT Month({sigma}.{field})) > {date.Month} ) or (((SELECT YEAR({sigma}.{field})) = {date.Year} and (SELECT Month({sigma}.{field})) = {date.Month} and (SELECT Day({sigma}.{field})) > {date.Day}))";
                // var lt = $"(({field}.Year < {date.Year}) || ({field}.Year == {date.Year} && {field}.Month < {date.Month} ) || ({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day < {date.Day}))";

                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"({eq})";
                        break;

                    case "neq":
                        exDate = $"not({eq})";
                        break;

                    case "gte":
                        exDate = $"({eq} or {gt})";
                        break;

                    case "gt":
                        exDate = $"({gt})";
                        break;

                    case "lte":
                        exDate = $"{eq} or not({gt})";
                        break;

                    case "lt":
                        exDate = $"not({eq}) and not({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "timespan" || dataType == "timespan?")
            {
                if (dataType == "timespan?")
                    field += "";

                var date = GetTime(param);

                var eq = $"({sigma}.{field}.{nameof(date.Hours)} == {date.Hours} and {sigma}.{field}.{nameof(date.Minutes)} = {date.Minutes} and {sigma}.{field}.{nameof(date.Seconds)} = {date.Seconds})";
                var gt = $"(({sigma}.{field}.{nameof(date.Hours)} > {date.Hours}) or ({sigma}.{field}.{nameof(date.Hours)} = {date.Hours} and {sigma}.{field}.{nameof(date.Minutes)}  > {date.Minutes} ) or ({sigma}.{field}.{nameof(date.Hours)}  = {date.Hours} and {sigma}.{field}.{nameof(date.Minutes)}  = {date.Minutes} and {sigma}.{field}.{nameof(date.Seconds)}  > {date.Seconds}))";
                // var lt = $"(({field}.{nameof(date.Hours)} < {date.Hours}) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)} < {date.Minutes} ) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)}  == {date.Minutes} && {field}.{nameof(date.Seconds)}  < {date.Seconds}))";


                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"{eq}";
                        break;

                    case "neq":
                        exDate = $"not({eq})";
                        break;

                    case "gte":
                        exDate = $"{eq} or {gt}";
                        break;

                    case "gt":
                        exDate = $"{gt}";
                        break;
                    case "lte":
                        exDate = $"{eq} or not({gt})";
                        break;

                    case "lt":
                        exDate = $"!({eq}) and not({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "boolean" || dataType == "boolean?")
            {
                if (param.ToLower() == Boolean.TrueString.ToLower())
                    param = Boolean.TrueString.ToLower();

                if (param.ToLower() == Boolean.FalseString.ToLower())
                    param = Boolean.FalseString.ToLower();
            }

            string exStr,
                caseMod = string.Empty;
            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} = N''{1}''", sigma + "." + field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} !=  N''{1}''", sigma + "." + field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("CONTAINS({0}{2}, N''{1}'')", sigma + "." + field, param, caseMod);
                    break;

                case "doesnotcontain":
                    exStr = string.Format("{0}{2} NOT LIKE N''%{1}%''", sigma + "." + field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2} LIKE N''{1}%''", sigma + "." + field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2} LIKE N''%{1}''", sigma + "." + field, param, caseMod);
                    break;

                case "gte":
                    exStr = string.Format("{0}{2} >=  N''{1}''", sigma + "." + field, param, caseMod);
                    break;

                case "gt":
                    exStr = string.Format("{0}{2} >  N''{1}''", sigma + "." + field, param, caseMod);
                    break;

                case "lte":
                    exStr = string.Format("{0}{2} <=  N''{1}''", field, param, caseMod);
                    break;

                case "lt":
                    exStr = string.Format("{0}{2} <  N''{1}''", sigma + "." + field, param, caseMod);
                    break;

                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }


        public static string GetExpressionDapper(FilterDescription filter)
        {

            if (filter == null)
                return null;

            if (filter.Filters == null || !filter.Filters.Any())
            {
                var exprList = GetExpressionDapper(filter.Field, filter.Operator, filter.Value);
                return exprList;
            }

            var logic = filter.Logic.ToLower() == "or" ? "or" : "and";

            var expressions = new List<string>();

            foreach (var item in filter.Filters)
            {
                var expr = GetExpressionDapper(item);

                if (!string.IsNullOrWhiteSpace(expr))
                    expressions.Add(expr);
            }

            if (expressions.Count == 0)
                return null;

            return "(" + string.Join(" " + logic + " ", expressions) + ")";
        }

    }


    public class FilterTreeList
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public List<FilterTreeList> Filters { get; set; }

        /// <summary>
        /// Mapping of Kendo DataSource filtering operators to Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"isnull", "="},
            {"isnotnull", "!="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"}
        };

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        public IList<FilterTreeList> All()
        {
            var filters = new List<FilterTreeList>();

            Collect(filters);

            return filters;
        }

        private void Collect(IList<FilterTreeList> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (FilterTreeList filter in Filters)
                {
                    filters.Add(filter);

                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// <summary>
        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 = @1 and Field2.Contains(@2)"
        /// </summary>
        /// <param name="filters">A list of flattened filters.</param>
        public string ToExpression(IList<FilterTreeList> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + String.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);

            string comparison = operators[Operator];


            if (Operator == "isnotnull" || Operator == "isnull")
            {
                return String.Format("{0} {1} null", Field, comparison);
            }

            if (Operator == "doesnotcontain")
            {
                return String.Format("!{0}.{1}(@{2})", Field, comparison, index);
            }

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return String.Format("{0}.{1}(@{2})", Field, comparison, index);
            }

            return String.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}
