using laget.Mapper.Core;
using laget.Mapper.Tests.Models;

namespace laget.Mapper.Tests.Mappers
{
    public class ReturnTypeVoidMapper : IMapper
    {
        [MapperMethod]
        public void PerformActionOnModel(Model model) => model.Id = 3;
    }
}