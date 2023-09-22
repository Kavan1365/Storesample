using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Services.Exceptions
{
    public static class PersianCultureExtensions
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }

        public static string ToPersianDateString(this DateTime date, string format = "yyyy/MM/dd")
        {
            return date.ToString(format, GetPersianCulture());
        }


        public static string ToPersianDate(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();
            return (persianCalendar.GetYear(dateTime) + "/" +
                   persianCalendar.GetMonth(dateTime).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(dateTime).ToString("00"));
        }

        public static string ToPersianDate(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToPersianDate();
            else
                return string.Empty;
        }

        public static string ToPersianDateTime(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();
            return (persianCalendar.GetYear(dateTime) + "/" +
                   persianCalendar.GetMonth(dateTime).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(dateTime).ToString("00") + " " +
                   persianCalendar.GetHour(dateTime).ToString("00") + ":" +
                   persianCalendar.GetMinute(dateTime).ToString("00"));
        }

        public static string ToPersianDateTime(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToPersianDateTime();
            else
                return string.Empty;
        }


        public static string ToPersianDayOfWeek(this DateTime date)
        {
            string week = "";
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    week = "جمعه";
                    break;
                case DayOfWeek.Saturday:
                    week = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    week = "یکشنبه";
                    break;
                case DayOfWeek.Monday:
                    week = "دوشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    week = "سه شنبه";
                    break;

                case DayOfWeek.Wednesday:
                    week = "چهار شنبه";
                    break;
                case DayOfWeek.Thursday:
                    week = "پنج شنبه";
                    break;
            }

            return week;
        }

        public static string ToPersianDayOfWeek(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToPersianDayOfWeek();
            else
                return string.Empty;


        }
    }
}
