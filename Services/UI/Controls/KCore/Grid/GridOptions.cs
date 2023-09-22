using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Services.UI.Controls.KCore.Grid
{
    public class GridOptions<TModel> : MvcControlAttributes
    {

       
        public GridOptions()
        {

            Columns = typeof(TModel).GetProperties().Select(p => new Column(p)).ToList();
          
            Columns.Where(x => x.Property.IsDefined(typeof(HiddenInputAttribute), true)).ToList()
                .ForEach(col => { col.Hidden = true; });
            Events = new Dictionary<string, string>();
        }

        internal override string Role
        {
            get { return "grid"; }
        }

        public bool FitColumns { get; set; } = true;

        public object Pageable { get; set; } = new
        {
            refresh = true,
            pageSizes = true,
            buttonCount = 5
        };

        public bool AutoBind { get; set; } = true;
        public bool ServerGrouping { get; set; } = true;
        public bool ServerSorting { get; set; } = true;
        public bool ServerAggregates { get; set; } = true;

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
        public bool InlineAdd { get; set; } = false;
        public string ReadUrl { get; set; }
        public string UpdateUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string Selectable { get; set; }
        internal List<Column> Columns { get; set; }
        internal List<ToolBarButton> ToolBarButtons { get; set; } = new List<ToolBarButton>();
        internal string GroupBy { get; set; }
        internal string GroupOrder { get; set; }

        internal string OrderBy { get; set; }
        internal string Order { get; set; }
        internal IEnumerable<GroupAggregate<TModel>> GroupAggregates { get; set; }
        public string CreateUrl { get; set; }
        public bool EditInModal { get; set; } = true;
        public string EditUrl { get; set; }
        public string DetailInit { get; set; }
        static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        public string Jwt { get; set; }

        public string DetailViewUrl { get; set; }
        public string DetailTemplate { get; set; }
        internal Dictionary<string, string> Events { get; set; }
        public InlineEdit InlineEdit { get; set; }
        public UpdateMode UpdateMode { get; set; }
        public bool Navigatable { get; set; } = false;


        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;
            attributes["source"] = JsonConvert.SerializeObject(GetDataSource(),
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            attributes["pageable"] = JsonConvert.SerializeObject(Pageable);
            attributes["scrollable"] = FitColumns.ToString().ToLower();
            attributes["sortable"] = Sortable.ToString().ToLower();
            attributes["auto-bind"] = AutoBind.ToString().ToLower();

            attributes["filterable"] = !FilterMode.HasFlag(FilterMode.None)
                ? JsonConvert.SerializeObject(new
                {
                    mode =
                        $"{(FilterMode.HasFlag(FilterMode.Menu) ? "menu" : "")}{((FilterMode.HasFlag(FilterMode.Menu) && FilterMode.HasFlag(FilterMode.Row)) ? "," : "")}{(FilterMode.HasFlag(FilterMode.Row) ? "row" : "")}",
                    extra = FilterMode.HasFlag(FilterMode.Extra)
                })
                : "false";

            attributes["reorderable"] = Reorderable.ToString().ToLower();
            attributes["column-menu"] = ColumnMenu.ToString().ToLower();
            attributes["groupable"] = Groupable.ToString().ToLower();
            attributes["resizable"] = (FitColumns ? Resizable : false).ToString().ToLower();
            attributes["allow-copy"] = AllowCopy.ToString().ToLower();
            attributes["inline-add"] = InlineAdd.ToString().ToLower();
            attributes["navigatable"] = Navigatable.ToString().ToLower();

            if (!string.IsNullOrEmpty(Selectable))
                attributes["selectable"] = Selectable;

            attributes["edit-in-modal"] = EditInModal.ToString().ToLower();

            foreach (var eventName in Events.Keys)
                attributes[eventName] = Events[eventName];

            if (DetailTemplate != null)
                attributes["detail-template"] = DetailTemplate;

            if (!string.IsNullOrEmpty(DetailInit))
                attributes["detail-init"] = DetailInit;

            else if (!string.IsNullOrEmpty(DetailViewUrl))
            {
                attributes["selectable"] = "row";
                attributes["change"] = "kendeHandlers.gridOnChange";
                attributes["detail-view-url"] = DetailViewUrl;
                attributes["detail-init"] = "kendeHandlers.gridDetailInit";
            }


            SetToolbarButtons(attributes);
            SetBaseCommands(attributes);

            attributes["columns"] = JsonConvert.SerializeObject(Columns);

        }

        private void SetBaseCommands(Dictionary<string, string> attributes)
        {
            string template = "";
            int width = 20;
            if (AllowDelete || !string.IsNullOrEmpty(AllowDeleteField))
            {
                width += 35;
                attributes["delete-url"] = DeleteUrl;
                template =
                    $"#if({AllowDeleteField ?? "true"}){{#<em title=\"حذف\" onclick=\"kendeHandlers.gridDataSourceDelete(event)\" data-role=\"button\" class=\"btn btn-sm btn-danger\" role=\"button\"><span class=\"k-icon k-i-delete\"></span></em>#}}#";
            }

            if (AllowEdit || !string.IsNullOrEmpty(AllowEditField))
            {
                width += 35;
                attributes["edit-url"] = EditUrl;
                template +=
                    $"#if({AllowEditField ?? "true"}){{#<em title=\"ویرایش\" onclick=\"kendeHandlers.gridDataSourceEdit(event)\" data-role=\"button\" class=\"btn btn-sm btn-info\" role=\"button\"><span class=\"k-icon k-i-edit\"></span></em>#}}#";
            }

            if (!string.IsNullOrEmpty(template))
            {
                Command[] commands = null;
                Columns.Insert(0, new Column(commands) { Width = $"{width}px", Template = template });
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
                if (item.ClientType== "date")
                {
                schemaFields.Add(
                    item.Field,
                    
                    new
                    {
                        type = item.ClientType,
                        editable = item.Editable
                    });
                    
                }
                else
                {
                schemaFields.Add(item.Field, new { type = item.ClientType, editable = item.Editable });

                }
                aggregates.AddRange(item.AggregatesValue.Select(ca => new
                {
                    field = item.Field,
                    aggregate = ca,
                }));
            }

            return new
            {
                //transport = new
                //{
                //    read = new { url = ReadUrl, dataType = "json", cache = false },
                //    update = InlineEdit != InlineEdit.None ? new { url = UpdateUrl, contentType = "application/json", dataType = "json", cache = false, method = "POST" } : null,
                //    create = InlineAdd ? new { url = UpdateUrl, contentType = "application/json", dataType = "json", cache = false, method = "POST" } : null,
                //    parameterMap = new JRaw("kendeHandlers.gridDataSourceParameterMap")
                //},

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
                            Authorization = "Bearer " + _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name

                        },
                        Timeout = 0,
                        //dataType = "json", 
                        cache = false
                    },
                    update = InlineEdit != InlineEdit.None
                           ? new
                           {
                               url = UpdateUrl,
                               contentType = "application/json",
                               dataType = "json",
                               cache = false,
                               headers = new
                               {
                                   Authorization = "Bearer " + _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name
                               },
                               Timeout = 0,

                               method = "POST"
                           }
                           : null,
                    create = InlineAdd
                           ? new
                           {
                               url = UpdateUrl,
                               contentType = "application/json",
                               dataType = "json",
                               headers = new
                               {
                                   Authorization = "Bearer " + _httpContextAccessor.HttpContext.User.Identities.Where(z => z.NameClaimType == ClaimTypes.Name).FirstOrDefault().Name

                               },
                               Timeout = 0,

                               cache = false,
                               method = "POST"
                           }
                           : null,
                    parameterMap = new JRaw("kendeHandlers.gridDataSourceParameterMap")
                },
                schema = new
                {
                    data = "Data",
                    total = "Total",
                    groups = "Group",
                    aggregates = "Aggregates",
                    errors = "Errors",
                    model = new { id = "Id", fields = schemaFields },
                    parse = new JRaw("kendeHandlers.gridDataSourceParser")

                },
                batch = UpdateMode != UpdateMode.None,
                autoSync = UpdateMode == UpdateMode.AutoSync,
                aggregate = aggregates,
                error = new JRaw("kendeHandlers.gridDataSourceError"),
                requestEnd = new JRaw("kendeHandlers.gridDataSourceRequestEnd"),
                pageSize = (Pageable is bool && (bool)Pageable == false) ? 0 : 10,
                sort = string.IsNullOrEmpty(OrderBy)
                    ? null
                    : new
                    {
                        field = OrderBy,
                        dir = Order,
                    },
                      serverPaging = true,
                      serverFiltering = true,
                      serverSorting = ServerSorting,
                      serverGrouping = ServerGrouping,
                      group = string.IsNullOrEmpty(GroupBy)
                    ? null
                    : new
                    {
                        field = GroupBy,
                        dir = GroupOrder,
                        aggregates = groupAggregates

                    },
                serverAggregates = ServerAggregates
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
                ToolBarButtons.Add(new ToolBarButton { Name = "createcustome",Text= Resources.Pages.CreateRecord, });
                attributes["create-url"] = CreateUrl;
            }

            if (InlineEdit != InlineEdit.None)
            {
                if (UpdateMode == UpdateMode.Batch)
                {
                    ToolBarButtons.Add(new ToolBarButton { Name = "save" });
                    ToolBarButtons.Add(new ToolBarButton { Name = "cancel" });
                }

                attributes["editable"] = InlineEdit == InlineEdit.Incell ? "incell" : "inline";
            }

            if (ToolBarButtons.Count > 0)
                attributes["toolbar"] = JsonConvert.SerializeObject(ToolBarButtons);
        }
    }

    [Flags]
    public enum FilterMode
    {
        Row = 1,
        Menu = 2,
        Extra = 4,
        None = 8
    }
}
