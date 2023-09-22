using BaseCore.Helper.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace BaseCore.Helper.DynamicLinq
{
    public class Group : SortTree
    {
        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }
    }

    /// <summary>
    /// Represents a sort expression of Mvc DataSource.
    /// </summary>
    [DataContract]
    public class SortTree
    {
        /// <summary>
        /// Gets or sets the name of the sorted field (property).
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the sort direction. Should be either "asc" or "desc".
        /// </summary>
        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Converts to form required by Dynamic Linq e.g. "Field1 desc"
        /// </summary>
        public string ToExpression()
        {
            return Field + " " + Dir;
        }
    }
    internal class Sort
    {

        public static string GetSortExpressionDapper<TModel>(List<SortDescription> sortDescriptions)
        {

            if (sortDescriptions == null || !sortDescriptions.Any())
                return null;

            var list = sortDescriptions.Select(sortDescription =>
                string.Format("{0} {1}", GetPropertyTypeSigma(typeof(TModel), sortDescription.field) + "." + sortDescription.field, sortDescription.dir)).ToList();
            var result = string.Join(" , ", list);
            return result;
        }

        public static string GetSortExpression(List<SortDescription> sortDescriptions)
        {

            if (sortDescriptions == null || !sortDescriptions.Any())
                return null;

            var list = sortDescriptions.Select(sortDescription =>
                string.Format("{0} {1}",  sortDescription.field, sortDescription.dir)).ToList();
            var result = string.Join(" , ", list);
            return result;
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
    }
}