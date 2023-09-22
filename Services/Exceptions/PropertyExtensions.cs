using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using Resources;

namespace Services.Exceptions
{
   
    public static class PropertyExtensions
    {

        public static MemberInfo GetMember<T>(this Expression<Func<T, object>> expression)
        {
            var mbody = expression.Body as MemberExpression;

            if (mbody != null) return mbody.Member;
            //This will handle Nullable<T> properties.
            var ubody = expression.Body as UnaryExpression;
            if (ubody != null)
            {
                mbody = ubody.Operand as MemberExpression;
            }
            if (mbody == null)
            {
                throw new ArgumentException("Expression is not a MemberExpression", "expression");
            }
            return mbody.Member;
        }

        public static PropertyInfo GetProperty<T>(this Expression<Func<T, object>> expression)
        {
            var member = GetMember(expression);
            if (member.MemberType == MemberTypes.Property)
                return (PropertyInfo)member;
            else
                return null;
        }


        public static string PropertyName<T>(this Expression<Func<T, object>> expression)
        {
            return GetMember(expression).Name;
        }

        public static string PropertyDisplay<T>(this Expression<Func<T, object>> expression)
        {
            var propertyMember = GetMember(expression);
            var displayAttributes = propertyMember.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            return displayAttributes.Length == 1 ? ((DisplayNameAttribute)displayAttributes[0]).DisplayName : propertyMember.Name;
        }

        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Resources.DataDictionary", typeof(DataDictionary).Assembly);
            var displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? propertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name;

            var resources = rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                .OfType<DictionaryEntry>().Select(x=>new {key=x.Key.ToString(),value=x.Value.ToString()}).ToList();


            var entry =
                resources
                    .Where(e => e.key == displayName)?.SingleOrDefault();

            var key = entry?.value;

                return (key !=null)? key:  displayName;
         

        }

        public static Type FindFieldType(this Type type, string fieldName, string parent = "", int dumpLevel = 3)
        {
            foreach (var property in type.GetProperties())
            {
                if (parent + property.Name == fieldName)
                    return property.PropertyType;

                if (parent.Split('.').Length > dumpLevel)
                    continue;

                if (isNestedProperty(property.PropertyType))
                {
                    var result = FindFieldType(property.PropertyType, fieldName, property.Name + ".");
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        private static bool isNestedProperty(Type type)
        {
            if (type.Assembly.FullName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase)) return false;
            return
                   (type.IsClass || type.IsInterface) &&
                   !type.IsValueType &&
                   !string.IsNullOrEmpty(type.Namespace) &&
                   !type.Namespace.StartsWith("System.", StringComparison.OrdinalIgnoreCase);
        }


      
    }

}
