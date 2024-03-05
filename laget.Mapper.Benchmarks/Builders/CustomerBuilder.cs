using laget.Mapper.Benchmarks.Models;
using System.Collections.Generic;

namespace laget.Mapper.Benchmarks.Builders
{
    public static class CustomerBuilder
    {
        public static Customer Make => new()
        {
            Id = 1,
            Name = "Eduardo Najera",
            Credit = 234.7m,
            Address = new Address
            {
                Id = 1,
                City = "istanbul",
                Country = "turkey",
                Street = "istiklal cad."
            },
            Addresses = new List<Address>
            {
                new()
                {
                    Id = 3,
                    City = "istanbul",
                    Country = "turkey",
                    Street = "istiklal cad."
                },
                new()
                {
                    Id = 4,
                    City = "izmir",
                    Country = "turkey",
                    Street = "konak"
                }
            }.ToArray(),
            HomeAddress = new Address
            {
                Id = 2,
                City = "istanbul",
                Country = "turkey",
                Street = "istiklal cad."
            },
            WorkAddresses = new List<Address>
            {
                new()
                {
                    Id = 5,
                    City = "istanbul",
                    Country = "turkey",
                    Street = "istiklal cad."
                },
                new()
                {
                    Id = 6,
                    City = "izmir",
                    Country = "turkey",
                    Street = "konak"
                }
            }
        };
    }
}
