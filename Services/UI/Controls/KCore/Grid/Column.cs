using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Services.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.UI.Controls.KCore.Grid
{
    public class Column
    {
          internal bool IsDate { get; private set; }
        internal PropertyInfo Property { get; private set; }

        internal Column(params Command[] commands)
        {
            Commands = commands;
            AllowFilter = false;
        }

        public Column(PropertyInfo property)
        {
            Property = property;
            Title = Property?.GetDisplayName();
            SetClientType();


            //TODO:Refactor
            if (Property.PropertyType.Name == "FileViewModel")
                Template = $"#=getFileLink({ Property.Name })#";

            if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                Template = $"#if({ property.Name }){{#<span class=\"badge bg-success\">بله</span>#}}else{{#<span class=\"badge bg-warning\">خیر</span>#}}#";
        }

        private void SetClientType()
        {
            var typeCode = Type.GetTypeCode(Property.PropertyType);
            if ((int)typeCode >= 5 && (int)typeCode <= 15)
            {
                if (typeCode == TypeCode.Int64)
                    Format = "{0:n0}";

                ClientType = "number";
            }
            else if (Property.PropertyType == typeof(string))
                ClientType = "string";
            else if (Property.PropertyType == typeof(DateTime) || Property.PropertyType == typeof(DateTime?))
            {
                //Format = "{0:yyyy/MM/dd}";
                //ClientType = "date";
                IsDate = true;
                Template = $"#if(" + Field + ") {# #=ToDateTimePicker(" + Field + ")# #}#";
               

            }
            else if (Property.PropertyType == typeof(bool) || Property.PropertyType == typeof(bool?))
                ClientType = "boolean";

            if (Property.PropertyType.IsEnum)
            {
                ClientType = "number";
                Values = Property.PropertyType.GetEnumDataSource().Select(e => new ColumnValue { Text = e.Value, Value = e.Key });
            }
        }

        [JsonProperty(PropertyName = "field", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get { return Property?.Name; } }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "template", NullValueHandling = NullValueHandling.Ignore)]
        public string Template { get; set; }

        [JsonIgnore]
        public ColumnAggregate Aggregates { get; set; }

        [JsonProperty(PropertyName = "aggregates")]
        internal IEnumerable<string> AggregatesValue
        {
            get
            {
                foreach (ColumnAggregate value in Enum.GetValues(typeof(ColumnAggregate)))
                    if (Aggregates.HasFlag(value))
                        yield return Enum.GetName(typeof(ColumnAggregate), value).ToLower();
            }
        }

        [JsonProperty(PropertyName = "footerTemplate")]
        public string FooterTemplate { get; set; }

        [JsonProperty(PropertyName = "groupFooterTemplate")]
        public string GroupFooterTemplate { get; set; }

        [JsonProperty(PropertyName = "groupHeaderTemplate")]
        public string GroupHeaderTemplate { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string ClientType { get; private set; }

        [JsonProperty(PropertyName = "values")]
        public IEnumerable<ColumnValue> Values { get; set; }

        [JsonProperty(PropertyName = "command", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Command> Commands { get; private set; }

        [JsonProperty(PropertyName = "width", NullValueHandling = NullValueHandling.Ignore)]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "hidden", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Hidden { get; set; }

        [JsonProperty(PropertyName = "editor", NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Editor { get; set; }

        [JsonIgnore]
        public bool? Editable { get; set; }

        [JsonIgnore]
        public bool AllowFilter { get; set; } = true;

        [JsonProperty(PropertyName = "filterable", NullValueHandling = NullValueHandling.Ignore)]
        internal object Filterable
        {
            get
            {
                if (!AllowFilter)
                    return false;
                if (IsDate)
                {
                    return new
                    {
                        ui= new JRaw("addMdDateTimePickerui"),
                    cell = new
                        {
                            showOperators = false,
                            @operator = ClientType == "string" ? "contains" : "eq",
                            minLength = 3,
                            delay = 1000
                        }
                    };
                }
                else
                {
                    return new
                    {
                        cell = new
                        {
                            showOperators = false,
                            @operator = ClientType == "string" ? "contains" : "eq",
                            minLength = 3,
                            delay = 1000
                        }
                    };
                }
               
            }
        }
    }
}
