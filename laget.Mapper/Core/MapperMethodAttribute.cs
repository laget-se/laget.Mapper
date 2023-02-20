using System;

namespace laget.Mapper.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MapperMethodAttribute : Attribute { }
}