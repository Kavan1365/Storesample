using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.UI.Controls.KCore.DropDownList
{
    public class DropDownListOptions : MvcControlAttributes
    {
        public DropDownListOptions()
        {
            Events = new Dictionary<string, string>();
        }

        internal override string Role { get { return Multiple ? "multiselect" : "dropdownlist"; } }
        public string Filter { get; set; } = "contains";
        public string OptionLabel { get; set; } = "انتخاب کنید...";
        public string DataTextField { get; set; } = "Title";
        public string DataValueField { get; set; } = "Id";
        public string DataGroupField { get; set; } = "Group";
        public string CascadeFrom { get; set; }
        public string CascadeFromField { get; set; }
        public bool Grouping { get; set; } = false;
        public bool Multiple { get; set; } = false;
        public string Jwt { get; set; }
        public bool Enum { get; set; } = false;
        public bool AutoBind { get; set; }
        public int MinLength { get; set; } = 1;
        public bool EnforceMinLength { get; set; } = false;
        internal System.Collections.IEnumerable LocalDataSource { get; set; }
        public string DataSourceUrl { get; set; }
        public string Method { get; set; } = "GEt";
        public bool ParameterMap { get; set; }
        public string CreateUrl { get; set; }
        public string Value { get; set; }
        internal Dictionary<string, string> Events { get; set; }
        static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;
            attributes.Add("value", Value);
            attributes.Add("filter", Filter);
            attributes.Add("cascade-from", CascadeFrom);
            attributes.Add("cascade-from-field", CascadeFromField);
            attributes.Add("option-label", OptionLabel);
            attributes.Add("multiple", Multiple.ToString().ToLower());
            attributes.Add("text-field", DataTextField);
            attributes.Add("value-field", DataValueField);
            attributes.Add("Enum", Enum.ToString().ToLower());

            attributes["min-length"] = MinLength.ToString();
            attributes["enforce-min-length"] = EnforceMinLength.ToString().ToLower();

            attributes["auto-bind"] = AutoBind.ToString().ToLower();

            foreach (var eventName in Events.Keys)
            {
                attributes[eventName] = Events[eventName];
            }

            if (!string.IsNullOrEmpty(CreateUrl))
            {
                attributes.Add("create-url", CreateUrl);
            }

            attributes["source"] = JsonConvert.SerializeObject(GetDataSource(),
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private object GetDataSource()
        {

            if (LocalDataSource == null)
                return new
                {
                    serverFiltering = true,
                    group = Grouping ? new { field = DataGroupField } : null,
                    transport = new
                    {
                        read = new { 
                            url = DataSourceUrl, 
                            method = "POST",
                            contentType = "application/json",
                            dataType = "json",
                            headers = new
                            {
                                Authorization = "Bearer " + _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name

                            },
                            Timeout = 0,
                            //dataType = "json", 
                            cache = false
                        },
                        parameterMap = new JRaw("kendeHandlers.gridDataSourceParameterMap")


                    },
                    schema = new
                    {
                        data = "Data",
                    }
                };

            else
                return new
                {
                    data = LocalDataSource,
                    group = Grouping ? new { field = DataGroupField } : null
                };

        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }


}
