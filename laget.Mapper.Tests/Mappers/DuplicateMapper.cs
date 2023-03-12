using laget.Mapper.Core;
using laget.Mapper.Tests.Models;

namespace laget.Mapper.Tests.Mappers
{
    public class DuplicateMapper : IMapper
    {
        [MapperMethod]
        public Dto ModelToDto(Model model) => new Dto { Id = model.Id * 10 };
    }
}