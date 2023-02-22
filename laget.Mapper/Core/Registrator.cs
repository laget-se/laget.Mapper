using Autofac;
using laget.Mapper.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace laget.Mapper.Core
{
    public interface IRegistrator
    {
        /// <summary>
        /// Add an Assembly to the scanning operation.
        /// </summary>
        /// <param name="assembly"></param>
        void Assembly(Assembly assembly);

        /// <summary>
        /// Add an Assembly by name to the scanning operation.
        /// </summary>
        /// <param name="assemblyName"></param>
        void Assembly(string assemblyName);

        /// <summary>
        /// Add the Assembly that contains type T to the scanning operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void AssemblyContainingType<T>();

        /// <summary>
        /// Add the calling Assembly to the scanning operation
        /// </summary>
        void TheCallingAssembly();

        /// <summary>
        /// Add the executing Assembly to the scanning operation with a
        /// custom type.
        /// </summary>
        void TheCallingAssembly<T>();
    }

    public class Registrator : IRegistrator
    {
        private readonly Dictionary<int, TypeReference> _bindings = new Dictionary<int, TypeReference>();
        private readonly ContainerBuilder _builder;

        public Registrator(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void Assembly(Assembly assembly)
        {
            var type = typeof(IMapper);
            var hash = TypeHash.Calculate(assembly.GetType(), type);

            if (!_bindings.ContainsKey(hash))
                _bindings.Add(hash, new TypeReference(assembly, type));
        }

        public void Assembly(string name)
        {
            Assembly(System.Reflection.Assembly.Load(new AssemblyName(name)), typeof(IMapper));
        }

        public void Assembly(string name, Type type)
        {
            Assembly(System.Reflection.Assembly.Load(new AssemblyName(name)), type);
        }

        public void AssemblyContainingType<T>()
        {
            var type = typeof(T);
            Assembly(type.GetTypeInfo().Assembly, type);
        }

        public void TheCallingAssembly()
        {
            Assembly(System.Reflection.Assembly.GetEntryAssembly(), typeof(IMapper));
        }

        public void TheCallingAssembly<T>()
        {
            Assembly(System.Reflection.Assembly.GetEntryAssembly(), typeof(T));
        }

        private void Assembly(Assembly assembly, Type type)
        {
            var hash = TypeHash.Calculate(assembly.GetType(), type);

            if (!_bindings.ContainsKey(hash))
                _bindings.Add(hash, new TypeReference(assembly, type));
        }


        /// <summary>
        /// Register all added type instances to Autofac.
        /// </summary>
        public void Register()
        {
            foreach (var (hash, reference) in _bindings)
            {
                _builder.RegisterAssemblyTypes(reference.Assembly)
                    .AssignableTo(reference.Type)
                    .AsImplementedInterfaces();
            }
        }

        private class TypeReference
        {
            public TypeReference(Assembly assembly, Type type)
            {
                Assembly = assembly;
                Type = type;
            }

            public Assembly Assembly { get; }

            public Type Type { get; }
        }
    }
}
