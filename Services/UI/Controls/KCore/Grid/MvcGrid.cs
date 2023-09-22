using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Services.UI.Controls.KCore.Grid
{
    public class MvcGrid<TModel> : MvcBaseControl
    {
        internal override string TagName { get { return "div"; } }

        internal override bool SelfClosing { get { return false; } }

        private GridOptions<TModel> Options { get; set; }

        public MvcGrid(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new GridOptions<TModel>();


            var urlHelperFactory = controlFactory.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(controlFactory.HtmlHelper.ViewContext);
        
            
            //UrlHelper urlHelper = new UrlHelper(controlFactory.HtmlHelper.ViewContext);
            Options.CreateUrl = urlHelper.Action("Create");
            Options.ReadUrl = urlHelper.Action("List");
            Options.UpdateUrl = urlHelper.Action("BatchEdit");
            Options.DeleteUrl = urlHelper.Action("Delete");
            Options.EditUrl = urlHelper.Action("Edit");
        }

        public MvcGrid<TModel> Config(Action<GridOptions<TModel>> configurator)
        {
            configurator.Invoke(this.Options);
            return this;
        }
        public MvcGrid<TModel> Column(Expression<Func<TModel, object>> expression, Action<Column> configurator)
        {
            var property = PropertyExtensions.GetProperty(expression);
            var column = Options.Columns.Find(c => c.Field == property.Name);
            configurator.Invoke(column);
            return this;
        }

        public MvcGrid<TModel> Column(Action<Column> configurator, params Command[] commands)
        {
            var column = new Column(commands.Count() == 0 ? null : commands);
            configurator?.Invoke(column);
            Options.Columns.Add(column);
            return this;
        }

        public MvcGrid<TModel> AddToolbarButton(ToolBarButton button)
        {
            Options.ToolBarButtons.Add(button);
            return this;
        }

        public MvcGrid<TModel> OrderBy(Expression<Func<TModel, object>> field, bool ascending)
        {
            var property = PropertyExtensions.GetProperty(field);
            Options.OrderBy = property.Name;
            Options.Order = ascending ? "asc" : "desc";
            return this;
        }
        public MvcGrid<TModel> GroupBy(Expression<Func<TModel, object>> field, bool ascending,
            params GroupAggregate<TModel>[] aggregates)
        {
            var property = PropertyExtensions.GetProperty(field);
            Options.GroupBy = property.Name;
            Options.GroupOrder = ascending ? "asc" : "desc";
            Options.GroupAggregates = aggregates;

            return this;
        }


        public MvcGrid<TModel> BindEvent(string eventName, string function)
        {
            Options.Events.Add(eventName, function);
            return this;
        }

        internal override MvcControlAttributes GetAttributes()
        {
            return Options;
        }

        internal override string GetContent()
        {
            return string.Empty;
        }
    }
}
