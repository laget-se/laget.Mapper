using System;
using System.Reflection;

namespace laget.Mapper.Exceptions
{
    public class MapperInvalidParametersException : InvalidOperationException
    {
        public MapperInvalidParametersException(MethodInfo mapperMethod)
            : base($"Method {mapperMethod.Name} of {mapperMethod.DeclaringType?.Name} is not a valid Mapper as it has {mapperMethod.GetParameters().Length} parameters when it should have 1")
        {
        }
    }
}
