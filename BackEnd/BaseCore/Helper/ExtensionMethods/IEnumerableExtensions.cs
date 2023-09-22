using BaseCore.BaseCore.ViewModel;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
namespace BaseCore.Helper.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static async IAsyncEnumerable<SelectItem> CastToSelectListItemAsync<T>(this IAsyncEnumerable<T> items, string value = "Id", string text = "Title")
        {

            await foreach (var item in items)
            {
                yield return new SelectItem("", "") { };
                //yield return new selects
                //{

                //    Title = item.GetType().GetProperty(text).GetValue(item).ToString(),
                //    Id = item.GetType().GetProperty(value).GetValue(item).ToString()
                //};
            }
        }



        public static async IAsyncEnumerable<ColumnValue> CastToColumnValueAsync<T>(this IAsyncEnumerable<T> items, string value = "Id", string text = "Title")
        {
            await foreach (var item in items)
            {
                yield return new ColumnValue
                {

                    Text = item.GetType().GetProperty(text).GetValue(item).ToString(),
                    Value = (int)item.GetType().GetProperty(value).GetValue(item)
                };
            }
        }


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
    public class ColumnValue
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

}
