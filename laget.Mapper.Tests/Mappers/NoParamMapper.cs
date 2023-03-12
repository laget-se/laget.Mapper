using laget.Mapper.Core;
using laget.Mapper.Tests.Models;

namespace laget.Mapper.Tests.Mappers
{
    public class NoParamMapper : IMapper
    {
        [MapperMethod]
        public Dto ReturnStaticDto() => new Dto { Id = 0 };
    }
}