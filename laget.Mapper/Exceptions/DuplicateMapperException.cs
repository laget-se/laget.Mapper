using System;
using System.Linq;
using System.Reflection;

namespace laget.Mapper.Exceptions
{
    public class DuplicateMapperException: InvalidOperationException
    {
        public DuplicateMapperException(MethodInfo mapperMethod)
            : base($"Multiple registration of mapper from {mapperMethod.GetParameters().First().ParameterType.Name} to {mapperMethod.ReturnType.Name}, {mapperMethod.Name} of {mapperMethod.DeclaringType?.Name} will be ignored")
        {
        }
    }
}
