using laget.Mapper.Core;
using laget.Mapper.Tests.Models;

namespace laget.Mapper.Tests.Mappers
{
    public class MultipleParamMapper : IMapper
    {
        [MapperMethod]
        public Dto DtoFromMultipleObjects(Model model, Entity entity) => new Dto { Id = model.Id + entity.Id };
    }
}