using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Utilities
{
    public static class ExtensionMethod
    {

        /// <summary>
        /// انتخاب کردن یک عنصر به صورت تصادفی
        /// </summary>
        /// <typeparam name="T">عنصر خروجی</typeparam>
        /// <param name="list">لیست ورودی</param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default(T);

            Random rnd = new Random();
            return list.ElementAt(rnd.Next(list.Count()));
        }

    }

    public static class UsefulExtensions
    {
        public static long ConvertToLastZore(this long digit)
        {
            if (digit < 0)
                digit = -digit;
            if (digit < 10)
                return digit;
            if (digit < 100)
                return digit;
            if (digit < 1000)
                return digit;

            var mod = digit % 1000;
            digit = digit - mod;
            return digit;
        }
    }
}
