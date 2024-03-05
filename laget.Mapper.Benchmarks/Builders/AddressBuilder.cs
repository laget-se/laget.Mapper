using laget.Mapper.Benchmarks.Models;

namespace laget.Mapper.Benchmarks.Builders
{
    public static class AddressBuilder
    {
        public static Address Make => new()
        {
            Id = 1,
            City = "istanbul",
            Country = "turkey",
            Street = "istiklal cad."
        };
    }
}
