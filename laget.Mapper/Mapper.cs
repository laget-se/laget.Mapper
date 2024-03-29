﻿using laget.Mapper.Core;
using laget.Mapper.Exceptions;
using laget.Mapper.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace laget.Mapper
{
    public static class Mapper
    {
        private static readonly ConcurrentDictionary<int, MapperMethodReference> Mappers = new ConcurrentDictionary<int, MapperMethodReference>();

        /// <summary>
        /// Register all mapper in the enumerable list.
        /// </summary>
        /// <param name="mappers">Implementations of IMapper</param>
        /// <exception cref="DuplicateMapperException">If the mapper is already registered DuplicateMapperException will be thrown.</exception>
        public static void RegisterMappers(IEnumerable<IMapper> mappers)
        {
            foreach (var mapper in mappers)
            {
                RegisterMapper(mapper);
            }
        }

        /// <summary>
        /// Register the supplied mapper.
        /// </summary>
        /// <param name="mapper">Implementation of IMapper</param>
        /// <exception cref="DuplicateMapperException">If the mapper is already registered DuplicateMapperException will be thrown.</exception>
        public static void RegisterMapper(IMapper mapper)
        {
            lock (Mappers)
            {
                var mapperMethods = GetMapperMethods(mapper);
                foreach (var mapperMethod in mapperMethods)
                {
                    var hash = TypeHash.Calculate(mapperMethod.GetParameters().First().ParameterType, mapperMethod.ReturnType);
                    if (Mappers.ContainsKey(hash))
                        throw new DuplicateMapperException(mapperMethod);

                    Mappers.TryAdd(hash, new MapperMethodReference(mapper, mapperMethod));
                }
            }
        }

        /// <summary>
        /// Register all mapper in the enumerable list.
        /// </summary>
        /// <param name="mappers">Implementations of IMapper</param>
        public static void TryRegisterMappers(IEnumerable<IMapper> mappers)
        {
            foreach (var mapper in mappers)
            {
                TryRegisterMapper(mapper);
            }
        }

        /// <summary>
        /// Register the supplied mapper.
        /// </summary>
        /// <param name="mapper">Implementation of IMapper</param>
        public static void TryRegisterMapper(IMapper mapper)
        {
            lock (Mappers)
            {
                var mapperMethods = GetMapperMethods(mapper);
                foreach (var mapperMethod in mapperMethods)
                {
                    try
                    {
                        var hash = TypeHash.Calculate(mapperMethod.GetParameters().First().ParameterType, mapperMethod.ReturnType);
                        if (Mappers.ContainsKey(hash))
                            continue;

                        if (Mappers.TryAdd(hash, new MapperMethodReference(mapper, mapperMethod)))
                        {
                            Debug.WriteLine($"Added mapper {nameof(mapper)} (Hash='{hash}')");
                        }
                        else
                        {
                            Debug.WriteLine($"Mapper {nameof(mapper)} was already added (Hash='{hash}')");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see
        /// cref="ConcurrentDictionary{TKey,TValue}"/>.
        /// </summary>
        public static int Count
        {
            get
            {
                lock (Mappers)
                {
                    return Mappers.Count;
                }
            }
        }

        /// <summary>
        /// Resets the internal Dictionary, for testing only
        /// </summary>
        public static void Reset()
        {
            lock (Mappers)
            {
                Mappers.Clear();
            }
        }

        public static TResult Map<TResult>(object source)
        {
            lock (Mappers)
            {
                var hash = TypeHash.Calculate(source.GetType(), typeof(TResult));
                if (!Mappers.TryGetValue(hash, out var mapper))
                    throw new MapperNotFoundException(source.GetType(), typeof(TResult));

                return (TResult)mapper.Map(source);
            }
        }

        public static TResult Map<TSource, TResult>(TSource source)
        {
            lock (Mappers)
            {
                var hash = TypeHash.Calculate<TSource, TResult>();
                if (!Mappers.TryGetValue(hash, out var mapper))
                    throw new MapperNotFoundException(source.GetType(), typeof(TResult));

                return (TResult)mapper.Map(source);
            }
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
                        throw new MapperReturnTypeVoidException(m);

                    var @params = m.GetParameters();
                    if (@params.Length != 1)
                        throw new MapperInvalidParametersException(m);

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