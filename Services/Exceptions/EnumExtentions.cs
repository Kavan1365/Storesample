using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Services.Helper;
using Services.Helper.Attribute;
using Resources;

namespace Services.Exceptions
{
    public static class EnumExtentions
    {

        public static string GetDisplay(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());


            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Resources.DataDictionary", typeof(DataDictionary).Assembly);
            var displayName = field.GetCustomAttribute<DisplayAttribute>();


            var resources = rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                .OfType<DictionaryEntry>().Select(x => new { key = x.Key.ToString(), value = x.Value.ToString() }).ToList();


            var entry =
                resources
                    .Where(e => e.key == displayName?.Name)?.SingleOrDefault();
            var getValue = "";
            if (entry != null)
            {
                getValue = entry?.value;
            }
            else
            {
                getValue = field.GetCustomAttribute<DisplayAttribute>()?.Name;

            }



            return getValue ?? field.Name;
        }

        public static string GetDisplayFlags(this Enum value)
        {
            return string.Join(", ", Enum.GetValues(value.GetType()).Cast<Enum>().Where(r => value.HasFlag(r)).Select(x => x.GetDisplay()).ToArray());
        }

        public static bool IgnoredInEditor(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return (field.IsDefined(typeof(IgnoreInEditorAttribute)));
        }



        public static Dictionary<int, string> GetEnumDataSource(this Type value)
        {
            var result = new Dictionary<int, string>();
            foreach (Enum p in Enum.GetValues(value))
            {
                result.Add(p.GetHashCode(), p.GetDisplay());
            }
            return result;
        }
    }
}
