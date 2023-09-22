using Services.UI.Controls.KCore.Grid;
using System.Collections;
using System.Collections.Generic;

namespace Services.Helper.CustomeExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable CastToSelectListItem<T>(this IEnumerable<T> items, string value = "Id", string text = "Title")
        {
            foreach (var item in items)
            {
                yield return new
                {
                    Title = item.GetType().GetProperty(text).GetValue(item).ToString(),
                    Id = item.GetType().GetProperty(value).GetValue(item)
                };
            }
        }

        public static IEnumerable<ColumnValue> CastToColumnValue<T>(this IEnumerable<T> items, string value = "Id", string text = "Title")
        {
            foreach (var item in items)
            {
                yield return new ColumnValue
                {
                    Text = item.GetType().GetProperty(text).GetValue(item).ToString(),
                    Value = (int)item.GetType().GetProperty(value).GetValue(item)
                };
            }
        }

        public static int Count(this IEnumerable source)
        {
            var col = source as ICollection;
            if (col != null)
                return col.Count;

            int c = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext())
                c++;
            return c;
        }
    }
}
