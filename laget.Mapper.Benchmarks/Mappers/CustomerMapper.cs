using laget.Mapper.Benchmarks.Models;
using laget.Mapper.Core;
using laget.Mapper.Extensions;
using System.Linq;

namespace laget.Mapper.Benchmarks.Mappers
{
    public class CustomerMapper : IMapper
    {
        [MapperMethod]
        public CustomerDto Map(Customer from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Address = from.Address.Map<AddressDto>(),
            HomeAddress = from.HomeAddress.Map<AddressDto>(),
            Addresses = from.Addresses.Map<AddressDto>().ToArray()
        };

        [MapperMethod]
        public Customer Map(CustomerDto from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Address = from.Address.Map<Address>(),
            HomeAddress = from.HomeAddress.Map<Address>(),
            Addresses = from.Addresses.Map<Address>().ToArray()
        };
    }
}
