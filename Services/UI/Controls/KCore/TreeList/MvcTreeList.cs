using System;
using System.Linq.Expressions;
using Services.Exceptions;
using Services.UI.Controls.KCore.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Services.UI.Controls.KCore.TreeList
{
    public class MvcTreeList<TModel> : MvcBaseControl
    {
        public MvcTreeList(ControlFactory controlFactory) : base(controlFactory)
        {
            Options = new TreeListOptions<TModel>();
            var urlHelperFactory = controlFactory.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(controlFactory.HtmlHelper.ViewContext);

            Options.CreateUrl = urlHelper.Action("Create");
            Options.ReadUrl = urlHelper.Action("List");
            Options.DeleteUrl = urlHelper.Action("Delete");
            Options.EditUrl = urlHelper.Action("Edit");
        }

        internal TreeListOptions<TModel> Options { get; private set; }

        internal override bool SelfClosing { get { return false; } }

        internal override string TagName { get { return "div"; } }

        public MvcTreeList<TModel> Config(Action<TreeListOptions<TModel>> configurator)
        {
            configurator.Invoke(this.Options);
            return this;
        }

        public MvcTreeList<TModel> Column(Expression<Func<TModel, object>> expression, Action<Column> configurator)
        {
            var property = PropertyExtensions.GetProperty(expression);
            var column = Options.Columns.Find(c => c.Field == property.Name);
            configurator.Invoke(column);
            return this;
        }

        public MvcTreeList<TModel> GroupBy(Expression<Func<TModel, object>> field, bool ascending,
            params GroupAggregate<TModel>[] aggregates)
        {
            var property = PropertyExtensions.GetProperty(field);
            Options.GroupBy = property.Name;
            Options.GroupOrder = ascending ? "asc" : "desc";
            Options.GroupAggregates = aggregates;

            return this;
        }

        public MvcTreeList<TModel> Column(Action<Column> configurator, params Command[] commands)
        {
            var column = new Column(commands);
            Options.Columns.Add(column);
            configurator?.Invoke(column);
            return this;
        }

        public MvcTreeList<TModel> BindEvent(string eventName, string function)
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
