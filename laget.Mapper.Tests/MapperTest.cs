using laget.Mapper.Core;
using laget.Mapper.Exceptions;
using laget.Mapper.Tests.Mappers;
using laget.Mapper.Tests.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace laget.Mapper.Tests
{
    public class MapperTest : IDisposable
    {
        public MapperTest()
        {
            Mapper.RegisterMappers(new[] { new TestMapper() });
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

        [Fact]
        public void RegisterMapperShouldBeThreadSafe()
        {
            var mappers = new List<IMapper>
            {
                new TestMapper()
            };
            var threads = new int[1000];

            Parallel.ForEach(threads, thread =>
            {
                Mapper.TryRegisterMappers(mappers);
            });

            Assert.NotEqual(0, Mapper.Count);
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
