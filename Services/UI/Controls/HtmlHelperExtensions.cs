using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.UI.Controls.KCore.DropDownList;
using Services.UI.Controls.KCore.Editor;
using Services.UI.Controls.KCore.Form;
using Services.UI.Controls.KCore.Grid;
using Services.UI.Controls.KCore.TreeList;
using Services.UI.Controls.KCore.TreeView;
using Services.UI.Controls.KCore.Window;
using System.Linq.Expressions;
using System.Reflection;

namespace Services.UI.Controls
{
    public static class HtmlHelperExtensions
    {
        public static ControlFactory Mvc(this IHtmlHelper helper)
        {
            return new ControlFactory(helper);
        }


        public static ControlFactory<TModel> Mvc<TModel>(this IHtmlHelper<TModel> helper)
        {
            return new ControlFactory<TModel>(helper);

        }
        public static MvcTreeList<TModel> TreeList<TModel>(this ControlFactory controlFactory, string name)
        {
            return new MvcTreeList<TModel>(controlFactory) { Name = name };
        }

        public static MvcTreeView TreeView(this ControlFactory controlFactory, string name)
        {
            return new MvcTreeView(controlFactory) { Name = name };
        }

        public static MvcTreeView TreeView(this ControlFactory controlFactory, string name, TreeViewAttribute treeViewAttribute)
        {
            var tree = new MvcTreeView(controlFactory) { Name = name };
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TreeViewAttribute, TreeViewOptions>();
                cfg.CreateMap<TreeViewOptions, TreeViewAttribute>();
            });
            var mapper = new Mapper(configuration);

            mapper.Map(treeViewAttribute, tree.Options);
            tree.Options.CheckBoxesInputName = name;
            return tree;

        }


        public static MvcGrid<TModel> Grid<TModel>(this ControlFactory controlFactory, string name)
        {
            return new MvcGrid<TModel>(controlFactory) { Name = name };
        }
        public static MvcDropDownList DropDownList(this ControlFactory controlFactory, string name)
        {
            return new MvcDropDownList(controlFactory) { Name = name };
        }

        public static MvcDropDownList DropDownList(this ControlFactory controlFactory, string name, DropDownListAttribute dropDownListAttribute)
        {
            var dropDownList = new MvcDropDownList(controlFactory) { Name = name };
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DropDownListAttribute, DropDownListOptions>();
                cfg.CreateMap<DropDownListOptions, DropDownListAttribute>();
            });

            var mapper = new Mapper(configuration);

            mapper.Map(dropDownListAttribute, dropDownList.Options);

            return dropDownList;
        }

        public static MvcDropDownList DropDownListFor<TModel, TProperty>(this ControlFactory<TModel> controlFactory, Expression<Func<TModel, TProperty>> expression)
        {

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member as PropertyInfo;

            return new MvcDropDownList(controlFactory) { Name = member.Name };
        }

        public static MvcFormButtons FormButtons(this ControlFactory controlFactory, string callBack = "", bool showSave = true, bool showCancel = true)
        {
            return new MvcFormButtons(controlFactory, callBack, showCancel, showSave) { Name = "formButtons" };
        }
        public static MvcWindow Window(this ControlFactory controlFactory, string name)
        {
            return new MvcWindow(controlFactory) { Name = name };
        }

        public static MvcEditor HtmlEditorFor<TModel, TProperty>(this ControlFactory<TModel> controlFactory, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                var member = ((MemberExpression)expression.Body).Member as PropertyInfo;
                var value = member.GetValue(controlFactory.HtmlHelper.ViewData.Model) as string;
                return new MvcEditor(controlFactory)
                {
                    Name = member.Name,
                    Value = value
                };
            }

            else
            {
                return new MvcEditor(controlFactory)
                {
                    Name = controlFactory.HtmlHelper.ViewData.ModelMetadata.PropertyName,
                    Value = controlFactory.HtmlHelper.ViewData.Model as string
                };

            }
            //var data = ModelMetadata.FromStringExpression(expression.Body.ToString(), controlFactory.HtmlHelper.ViewData);
        }


    }
}
