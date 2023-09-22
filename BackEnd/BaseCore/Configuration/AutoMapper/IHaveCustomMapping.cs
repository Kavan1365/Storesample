using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace BaseCore.Configuration.AutoMapper
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}
