using System;

namespace BaseCore.Configuration.AutoMapper
{
    public class NestedFieldAttribute : Attribute
    {
        public string Chain { get; set; }
        public NestedFieldAttribute(string chain)
        {
            Chain = chain;
        }
    }
}
