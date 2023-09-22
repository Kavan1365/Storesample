using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Services.UI.Controls.KCore.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DocumentFormat.OpenXml.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.UI.Controls.KCore.TreeList
{
    public class TreeListOptions<TModel> : MvcControlAttributes
    {
        internal override string Role { get { return "treelist"; } }

        public TreeListOptions()
        {
            Columns = typeof(TModel).GetProperties().Select(p => new Column(p)).ToList();
            Columns.Where(x => x.Property.IsDefined(typeof(HiddenInputAttribute), true)).ToList().ForEach(col => { col.Hidden = true; });
            Events = new Dictionary<string, string>();
        }

        public bool Scrollable { get; set; } = true;
        public bool Sortable { get; set; } = true;
        public FilterMode FilterMode { get; set; } = FilterMode.Menu;
        public bool Reorderable { get; set; } = true;
        public bool ColumnMenu { get; set; } = false;
        public bool Groupable { get; set; } = false;
        public bool Resizable { get; set; } = true;
        public bool AllowCopy { get; set; } = false;
        public bool AllowDelete { get; set; } = true;
        public string AllowDeleteField { get; set; }
        public bool AllowEdit { get; set; } = true;
        public string AllowEditField { get; set; }
        public bool AllowExportToExcel { get; set; } = true;
        public bool ShowAddButton { get; set; } = true;
        public string ReadUrl { get; set; }
        public string DeleteUrl { get; set; }
        public bool EditInModal { get; set; } = true;
        public string Selectable { get; set; }
        internal List<Column> Columns { get; set; }
        internal List<ToolBarButton> ToolBarButtons { get; set; } = new List<ToolBarButton>();
        static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        internal string GroupBy { get; set; }
        internal string GroupOrder { get; set; }
        internal IEnumerable<GroupAggregate<TModel>> GroupAggregates { get; set; }
        public string CreateUrl { get; set; }
        public string EditUrl { get; set; }
        public int? ParentDefultValue { get; set; }
        internal Dictionary<string, string> Events { get; set; }

        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;
            attributes["source"] = JsonConvert.SerializeObject(GetDataSource(),
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            attributes["scrollable"] = Scrollable.ToString().ToLower();
            attributes["sortable"] = Sortable.ToString().ToLower();

            attributes["filterable"] = !FilterMode.HasFlag(FilterMode.None) ? JsonConvert.SerializeObject(new
            {
                mode = $"{(FilterMode.HasFlag(FilterMode.Menu) ? "menu" : "")}{((FilterMode.HasFlag(FilterMode.Menu) && FilterMode.HasFlag(FilterMode.Row)) ? "," : "")}{(FilterMode.HasFlag(FilterMode.Row) ? "row" : "")}",
                extra = FilterMode.HasFlag(FilterMode.Extra)
            }) : "false";

            foreach (var eventName in Events.Keys)
            {
                attributes[eventName] = Events[eventName];
            }

            attributes["reorderable"] = Reorderable.ToString().ToLower();
            attributes["column-menu"] = ColumnMenu.ToString().ToLower();
            attributes["groupable"] = Groupable.ToString().ToLower();
            attributes["resizable"] = Resizable.ToString().ToLower();
            attributes["allow-copy"] = AllowCopy.ToString().ToLower();
            if (!string.IsNullOrEmpty(Selectable))
                attributes["selectable"] = Selectable;
            attributes["editabe"] = "false";
            attributes["edit-in-modal"] = EditInModal.ToString().ToLower();
            SetToolbarButtons(attributes);
            SetBaseCommands(attributes);
            attributes["selectable"] = "row";
            attributes["change"] = "kendeHandlers.treeListOnChange";
            attributes["columns"] = JsonConvert.SerializeObject(Columns);
        }

        private void SetBaseCommands(Dictionary<string, string> attributes)
        {
            string template = "";

            if (AllowDelete || !string.IsNullOrEmpty(AllowDeleteField))
            {
                attributes["delete-url"] = DeleteUrl;
                template = $"#if({ AllowDeleteField ?? "true" }){{#<em title=\"حذف\" onclick=\"kendeHandlers.gridDataSourceDelete(event)\" data-role=\"button\" class=\"k-button k-button-icon\" role=\"button\"><span class=\"k-icon k-i-delete\"></span></em>#}}#";
            }
            if (AllowEdit || !string.IsNullOrEmpty(AllowEditField))
            {
                attributes["edit-url"] = EditUrl;
                template += $"#if({ AllowEditField ?? "true" }){{#<em title=\"ویرایش\" onclick=\"kendeHandlers.gridDataSourceEdit(event)\" data-role=\"button\" class=\"k-button k-button-icon\" role=\"button\"><span class=\"k-icon k-i-edit\"></span></em>#}}#";
            }

            if (!string.IsNullOrEmpty(template))
            {
                Command[] commands = null;
                Columns.Add(new Column(commands) { Width = "85px", Template = template });
            }
        }

        private object GetDataSource()
        {
            var schemaFields = new Dictionary<dynamic, dynamic>();
            var aggregates = new List<dynamic>();
            var groupAggregates = new List<dynamic>();

            if (GroupAggregates != null)
                foreach (var item in GroupAggregates)
                {
                    groupAggregates.AddRange(item.AggregatesValue.Select(ca => new
                    {
                        field = item.Field,
                        aggregate = ca,
                    }));
                }

            foreach (var item in Columns.Where(c => !string.IsNullOrEmpty(c.Field)))
            {
                if (item.Field == "ParentId")
                {
                    if (ParentDefultValue.HasValue)
                        schemaFields.Add(item.Field, new { defaultValue = ParentDefultValue });
                    else
                        schemaFields.Add(item.Field, new { nullable = true });
                }
                else
                {
                    schemaFields.Add(item.Field, new { type = item.ClientType });

                }
                aggregates.AddRange(item.AggregatesValue.Select(ca => new
                {
                    field = item.Field,
                    aggregate = ca,
                }));
            }

            return new
            {
                transport = new
                    {
                read = new
                {
                    url = ReadUrl,
                    method = "POST",
                    contentType = "application/json",
                    dataType = "json",
                    headers = new
                    {
                        Authorization = "Bearer " + _httpContextAccessor.HttpContext.User.Claims
         .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value
                    },
                    cache = false
                },
                    parameterMap = new JRaw("kendeHandlers.gridDataSourceParameterMap")
                },
                schema = new
                {
                    data = "Data",
                    total = "Total",
                    groups = "Group",
                    aggregates = "Aggregates",
                    errors = "Errors",
                    model = new { id = "Id", parentId = "ParentId", fields = schemaFields },
                    parse = new JRaw("kendeHandlers.gridDataSourceParser")

                },
                aggregate = aggregates,
                error = new JRaw("kendeHandlers.gridDataSourceError"),
                serverFiltering = true,
                serverSorting = true,
                serverGrouping = true,
                group = string.IsNullOrEmpty(GroupBy) ? null : new
                {
                    field = GroupBy,
                    dir = GroupOrder,
                    aggregates = groupAggregates

                },
                serverAggregates = true
            };
    }

    private void SetToolbarButtons(Dictionary<string, string> attributes)
    {
        if (AllowExportToExcel)
        {
            attributes["excel"] = JsonConvert.SerializeObject(new { allPages = true, filterable = true });
            attributes["excel-export"] = "kendeHandlers.gridExcelExport";
            ToolBarButtons.Add(new ToolBarButton { Name = "excel" });
        }
        if (ShowAddButton)
        {
            attributes["create-url"] = CreateUrl;
                ToolBarButtons.Add(new ToolBarButton { Name = "createcustome",Text= Resources.Pages.CreateRecord, });
        }

        if (ToolBarButtons.Count > 0)
            attributes["toolbar"] = JsonConvert.SerializeObject(ToolBarButtons);
    }
}
}
