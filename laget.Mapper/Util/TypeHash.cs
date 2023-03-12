using System;

namespace laget.Mapper.Util
{
    public static class TypeHash
    {
        public static int Calculate<T1, T2>() => HashCode.Combine(typeof(T1).GetHashCode(), typeof(T2).GetHashCode());
        public static int Calculate<T1>(Type t2) => HashCode.Combine(typeof(T1).GetHashCode(), t2.GetHashCode());
        public static int Calculate(string an, Type t2) => HashCode.Combine(an.GetHashCode(), t2.GetHashCode());
        public static int Calculate(Type t1, Type t2) => HashCode.Combine(t1.GetHashCode(), t2.GetHashCode());
    }
}