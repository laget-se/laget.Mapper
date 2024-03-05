using System.Collections.Generic;

namespace laget.Mapper.Benchmarks.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Credit { get; set; }
        public Address Address { get; set; }
        public Address HomeAddress { get; set; }
        public Address[] Addresses { get; set; }
        public List<Address> WorkAddresses { get; set; }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public AddressDto HomeAddress { get; set; }
        public AddressDto[] Addresses { get; set; }
        public List<AddressDto> WorkAddresses { get; set; }
        public string AddressCity { get; set; }
    }
}
