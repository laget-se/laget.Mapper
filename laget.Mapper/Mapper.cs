using laget.Mapper.Core;
using laget.Mapper.Exceptions;
using laget.Mapper.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace laget.Mapper
{
    public static class Mapper
    {
        private static ILogger<Mappings> _logger;
        private static readonly Dictionary<int, MapperMethodReference> Mappers = new Dictionary<int, MapperMethodReference>();

        public static void RegisterLogger(ILogger<Mappings> logger = null)
        {
            _logger = logger ?? NullLogger<Mappings>.Instance;
        }

        public static void RegisterMappers(IEnumerable<IMapper> mappers)
        {
            foreach (var mapper in mappers)
            {
                LoadMapper(mapper);
            }
        }

        private static void LoadMapper(IMapper mapper)
        {
            var mapperMethods = GetMapperMethods(mapper);
            foreach (var mapperMethod in mapperMethods)
            {
                var hash = MapperHash.Calculate(mapperMethod.GetParameters().First().ParameterType, mapperMethod.ReturnType);
                if (Mappers.ContainsKey(hash))
                {
                    _logger?.LogError($"Multiple registration of mapper from {mapperMethod.GetParameters().First().ParameterType.Name} to {mapperMethod.ReturnType.Name}, {mapperMethod.Name} of {mapperMethod.DeclaringType?.Name} will be ignored");
                    continue;
                }

                Mappers.Add(hash, new MapperMethodReference(mapper, mapperMethod));
            }
        }

        public static TResult Map<TResult>(object source)
        {
            var hash = MapperHash.Calculate(source.GetType(), typeof(TResult));
            if (!Mappers.TryGetValue(hash, out var mapper))
                throw new MapperNotFoundException(source.GetType(), typeof(TResult));

            return (TResult)mapper.Map(source);
        }

        public static TResult Map<TSource, TResult>(TSource source)
        {
            var hash = MapperHash.Calculate<TSource, TResult>();
            if (!Mappers.TryGetValue(hash, out var mapper))
                throw new MapperNotFoundException(source.GetType(), typeof(TResult));

            return (TResult)mapper.Map(source);
        }

        private static IEnumerable<MethodInfo> GetMapperMethods(IMapper mapper) =>
            mapper
                .GetType()
                .GetMethods()
                .Where(m =>
                {
                    if (m.IsPrivate || !m.GetCustomAttributes(typeof(MapperMethodAttribute), false).Any())
                        return false;

                    var returns = m.ReturnType;
                    if (returns == typeof(void))
                        _logger?.LogError($"Method {m.Name} of {m.DeclaringType?.Name} is not a valid Mapper as it returns void");

                    var @params = m.GetParameters();
                    if (@params.Length != 1)
                        _logger?.LogError($"Method {m.Name} of {m.DeclaringType?.Name} is not a valid Mapper as it has {@params.Length} parameters when it should have 1");

                    return returns != typeof(void) && @params.Length == 1;
                });

        private class MapperMethodReference
        {
            private readonly MethodInfo _mapperMethod;
            private readonly IMapper _mapperInstance;

            public MapperMethodReference(IMapper mapperInstance, MethodInfo mapperMethod)
            {
                _mapperInstance = mapperInstance;
                _mapperMethod = mapperMethod;
            }

            public object Map(object source) =>
                _mapperMethod.Invoke(_mapperInstance, new[] { source });
        }
    }
}