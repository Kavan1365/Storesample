using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Services.Exceptions;

namespace Services.UI.Controls.KCore.Grid
{
    public class GroupAggregate<TModel>
    {
        internal string Field { get; set; }
        internal ColumnAggregate Aggregates { get; set; }
        internal IEnumerable<string> AggregatesValue
        {
            get
            {
                foreach (ColumnAggregate value in Enum.GetValues(typeof(ColumnAggregate)))
                    if (Aggregates.HasFlag(value))
                        yield return Enum.GetName(typeof(ColumnAggregate), value).ToLower();
            }
        }
        public GroupAggregate(Expression<Func<TModel, object>> field, ColumnAggregate aggregates)
        {
            Field = PropertyExtensions.GetProperty(field).Name;
            Aggregates = aggregates;

        }
    }
}
