using laget.Mapper.Benchmarks.Models;
using laget.Mapper.Core;

namespace laget.Mapper.Benchmarks.Mappers
{
    public class AddressMapper : IMapper
    {
        [MapperMethod]
        public AddressDto Map(Address from) => new()
        {
            Id = from.Id,
            Street = from.Street,
            ZipCode = from.ZipCode,
            City = from.City,
            Country = from.Country
        };

        [MapperMethod]
        public Address Map(AddressDto from) => new()
        {
            Id = from.Id,
            Street = from.Street,
            ZipCode = from.ZipCode,
            City = from.City,
            Country = from.Country
        };
    }
}
