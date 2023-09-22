using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace BaseCore.Helper
{
    public class PersianCultureHelper
    {
        public static NumberFormatInfo FixPersianNumberFormat()
        {
            return new NumberFormatInfo
            {
                CurrencySymbol = "ريال",
                CurrencyDecimalSeparator = ".",
                NumberDecimalSeparator = ".",
                PercentDecimalSeparator = ".",
                DigitSubstitution = DigitShapes.NativeNational
            };
        }

        public static DateTimeFormatInfo FixPersianDateTimeFormat()
        {
            FieldInfo dateTimeFormatInfoReadOnly = typeof(DateTimeFormatInfo).GetField("m_isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo dateTimeFormatInfoCalendar = typeof(DateTimeFormatInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance); ;

            var info = new DateTimeFormatInfo();

            bool readOnly = (bool)dateTimeFormatInfoReadOnly.GetValue(info);
            if (readOnly)
            {
                dateTimeFormatInfoReadOnly.SetValue(info, false);
            }
            dateTimeFormatInfoCalendar.SetValue(info, new PersianCalendar());
            if (info.Calendar.GetType() == typeof(PersianCalendar))
            {
                info.AbbreviatedDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                info.ShortestDayNames = new string[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                info.DayNames = new string[] { "یکشنبه", "دوشنبه", "ﺳﻪشنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
                info.AbbreviatedMonthNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
                info.MonthNames = new string[] { "فروردین", "ارديبهشت", "خرداد", "تير", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };
                info.AMDesignator = "ق.ظ";
                info.PMDesignator = "ب.ظ";
                info.FirstDayOfWeek = DayOfWeek.Saturday;
                info.FullDateTimePattern = "yyyy MMMM dddd, dd HH:mm:ss";
                info.LongDatePattern = "yyyy MMMM dddd, dd";
                info.ShortDatePattern = "yyyy/MM/dd";
            }
            if (readOnly)
            {
                dateTimeFormatInfoReadOnly.SetValue(info, true);
            }
            return info;
        }
        public static CultureInfo FixPersianCulture(CultureInfo culture)
        {
            PropertyInfo calendarID = typeof(Calendar).GetProperty("ID", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo cultureInfoReadOnly = typeof(CultureInfo).GetField("m_isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo cultureInfoCalendar = typeof(CultureInfo).GetField("calendar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            //FieldInfo cultureInfoReadOnly = typeof(CultureInfo).GetField("m_", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (culture == null)
                culture = new CultureInfo("fa-IR", false);
            if (culture == null || culture.LCID != 1065)
                return culture;

            FixOptionalCalendars(culture, 4);
            culture = new CultureInfo("fa-IR", false);

            culture.NumberFormat = FixPersianNumberFormat();

            bool readOnly = (bool)cultureInfoReadOnly.GetValue(culture);
            if (readOnly)
            {
                cultureInfoReadOnly.SetValue(culture, false);
            }
            culture.DateTimeFormat = FixPersianDateTimeFormat();
            cultureInfoCalendar.SetValue(culture, new PersianCalendar());
            if (readOnly)
            {
                cultureInfoReadOnly.SetValue(culture, true);
            }
            return culture;
        }
        public static CultureInfo FixOptionalCalendars(CultureInfo culture, int CalenadrIndex)
        {
            FieldInfo cultureDataField = typeof(CultureInfo).GetField("m_cultureData",
              BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Object cultureData = cultureDataField?.GetValue(culture);
            FieldInfo waCalendarsField = cultureData?.GetType().GetField("waCalendars",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int[] waCalendars = (int[])waCalendarsField?.GetValue(cultureData);
            if (waCalendars != null && CalenadrIndex >= 0 && CalenadrIndex < waCalendars.Length)
                waCalendars[CalenadrIndex] = 0x16;
            waCalendarsField?.SetValue(cultureData, waCalendars);
            return culture;
        }
    }

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

        public static string GetDayFarsi(this DateTime datetime)
        {
            PersianCalendar pc = new PersianCalendar();
            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "یک شنبه";
                case DayOfWeek.Monday:
                    return "دو شنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهار شنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه ";
                case DayOfWeek.Saturday:
                    return "شنبه ";
                default:
                    return "";
            }
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
        public static string ToPersianTime(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();
            return ( persianCalendar.GetHour(dateTime).ToString("00") + ":" +
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
