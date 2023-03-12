using laget.Mapper.Core;
using laget.Mapper.Tests.Models;

namespace laget.Mapper.Tests.Mappers
{
    public class TestMapper : IMapper
    {
        [MapperMethod]
        public Dto ModelToDto(Model model) => new Dto { Id = model.Id };

        [MapperMethod]
        public Dto ModelBaseToDto(ModelBase modelBase) => new Dto { Id = 100 * modelBase.Id };

        [MapperMethod]
        private Model EntityToModel(Entity entity) => new Model { Id = entity.Id };

        public Entity ModelToEntity(Model model) => new Entity { Id = model.Id };
    }
}