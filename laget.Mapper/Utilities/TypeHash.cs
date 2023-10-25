using System;
using System.Reflection;

namespace laget.Mapper.Utilities
{
    internal static class TypeHash
    {
        public static int Calculate<T1, T2>() => HashCode.Combine(typeof(T1).GetHashCode(), typeof(T2).GetHashCode());
        public static int Calculate<T1>(Type t2) => HashCode.Combine(typeof(T1).GetHashCode(), t2.GetHashCode());
        public static int Calculate(Type t1, Type t2) => HashCode.Combine(t1.GetHashCode(), t2.GetHashCode());
        public static int Calculate(Assembly a, Type t) => HashCode.Combine(a.GetHashCode(), t.GetHashCode());
    }
}