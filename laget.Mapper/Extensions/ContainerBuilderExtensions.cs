using Autofac;
using laget.Mapper.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace laget.Mapper.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterMaps(this ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<IMapper>()
                .AsImplementedInterfaces();

            builder.RegisterBuildCallback(c =>
            {
                var logger = c.Resolve<ILogger<Mappings>>();
                var mappers = c.Resolve<IEnumerable<IMapper>>();

                Mapper.RegisterLogger(logger);
                Mapper.RegisterMappers(mappers);
            });
        }
    }
}
