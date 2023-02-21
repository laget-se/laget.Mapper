using laget.Mapper.Core;
using laget.Mapper.Exceptions;
using System;
using Xunit;

namespace laget.Mapper.Tests
{
    public class MapperTest : IDisposable
    {
        public MapperTest()
        {
            Mapper.RegisterMappers(new[] { new TestMapper() });
        }

        public void Dispose()
        {
            Mapper.Reset();
        }

        [Fact]
        public void MapperProperlyPicksUpMapperFunction()
        {
            var model = new Model { Id = 1 };

            var dto = Mapper.Map<Dto>(model);

            Assert.Equal(1, dto.Id);
        }

        [Fact]
        public void MapperCanProperlyMapAgainstSpecificBaseClass()
        {
            var model = new Model { Id = 1 };

            var dto = Mapper.Map<ModelBase, Dto>(model);

            Assert.Equal(100, dto.Id);
        }

        [Fact]
        public void MapperThrowsExceptionWhenNoMapperExists()
        {
            var dto = new Dto { Id = 1 };

            Assert.Throws<MapperNotFoundException>(() => Mapper.Map<Model>(dto));
        }

        [Fact]
        public void MapperDoesNotPickUpPrivateMethods()
        {
            var entity = new Entity { Id = 1 };

            Assert.Throws<MapperNotFoundException>(() => Mapper.Map<Model>(entity));
        }

        [Fact]
        public void MapperDoesNotPickUpMethodsWithoutAttribute()
        {
            var model = new Model { Id = 1 };

            Assert.Throws<MapperNotFoundException>(() => Mapper.Map<Entity>(model));
        }

        [Fact]
        public void MapperThrowsExceptionWhenRegisteringDuplicateMappingFunctions()
        {
            Assert.Throws<DuplicateMapperException>(() => Mapper.RegisterMappers(new[] { new DuplicateMapper() }));
        }

        [Fact]
        public void MapperThrowsExceptionWhenRegisteringMappingFunctionsWithVoidReturn()
        {
            Assert.Throws<MapperReturnTypeVoidException>(() => Mapper.RegisterMappers(new[] { new ReturnTypeVoidMapper() }));
        }

        [Fact]
        public void MapperThrowsExceptionWhenRegisteringMappingFunctionsWithNoParameters()
        {
            Assert.Throws<MapperInvalidParametersException>(() => Mapper.RegisterMappers(new[] { new NoParamMapper() }));
        }

        [Fact]
        public void MapperThrowsExceptionWhenRegisteringMappingFunctionsWithMultipleParameters()
        {
            Assert.Throws<MapperInvalidParametersException>(() => Mapper.RegisterMappers(new[] { new MultipleParamMapper() }));
        }
    }

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

    public class DuplicateMapper : IMapper
    {
        [MapperMethod]
        public Dto ModelToDto(Model model) => new Dto { Id = model.Id * 10 };
    }

    public class ReturnTypeVoidMapper : IMapper
    {
        [MapperMethod]
        public void PerformActionOnModel(Model model) => model.Id = 3;
    }

    public class NoParamMapper : IMapper
    {
        [MapperMethod]
        public Dto ReturnStaticDto() => new Dto { Id = 0 };
    }

    public class MultipleParamMapper : IMapper
    {
        [MapperMethod]
        public Dto DtoFromMultipleObjects(Model model, Entity entity) => new Dto { Id = model.Id + entity.Id };
    }

    public class ModelBase
    {
        public int Id { get; set; }
    }

    public class Model : ModelBase
    {
    }

    public class Dto
    {
        public int Id { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }
    }
}
