using System;

namespace laget.Mapper.Exceptions
{
    public class MapperNotFoundException : InvalidOperationException
    {
        public MapperNotFoundException(Type source, Type result)
            : base($"No mappers available to perform {source.FullName} -> {result.FullName}")
        {
        }
    }
}
