﻿using AutoMapper;
using BaseCore.Configuration.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Api.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void InitializeAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {

            services.AddAutoMapper(config =>
            {
                config.AddCustomMappingProfile(assemblies);
                //config.Advanced.BeforeSeal(configProvicer =>
                //{
                //    configProvicer.CompileMappings();
                //});
            }, assemblies);



        }


        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            var list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
                                              type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
                .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

            var profile = new CustomMappingProfile(list);


            config.AddProfile(profile);
        }
    }





}
