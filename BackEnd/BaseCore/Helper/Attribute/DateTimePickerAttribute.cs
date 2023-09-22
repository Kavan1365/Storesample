using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCore.Helper.Attribute
{
    public class DateTimePickerAttribute : System.Attribute
    {
        public DateTimePickerType Type { get; set; }
        public string StartField { get; set; }
        public DateTimePickerAttribute(DateTimePickerType type = DateTimePickerType.Date, string startField = null)
        {
            Type = type;
            StartField = startField;
        }
    }

    public enum DateTimePickerType
    {
        Date,
        DateTime,
        Time
    }
}
