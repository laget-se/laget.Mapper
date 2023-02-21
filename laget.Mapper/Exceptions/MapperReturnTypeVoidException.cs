using System;
using System.Reflection;

namespace laget.Mapper.Exceptions
{
    public class MapperReturnTypeVoidException : InvalidOperationException
    {
        public MapperReturnTypeVoidException(MethodInfo mapperMethod)
            : base($"Method {mapperMethod.Name} of {mapperMethod.DeclaringType?.Name} is not a valid Mapper as it returns void")
        {
        }
    }
}
