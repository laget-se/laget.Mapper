using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using laget.Mapper.Benchmarks.Builders;
using laget.Mapper.Benchmarks.Mappers;
using laget.Mapper.Benchmarks.Models;

namespace laget.Mapper.Benchmarks
{
    [MemoryDiagnoser]
    [MeanColumn, MinColumn, MaxColumn, MedianColumn]
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.Net80, 2, 10, 50)]
    public class MappingBenchmarks
    {
        [GlobalSetup]
        public void Setup()
        {
            Mapper.RegisterMapper(new AddressMapper());
            Mapper.RegisterMapper(new CustomerMapper());
        }

        [Benchmark]
        public void Benchmark_Simple_Mapper()
        {
            var model = AddressBuilder.Make;

            Mapper.Map<AddressDto>(model);
        }

        [Benchmark]
        public void Benchmark_DeepType_Mapper()
        {
            var model = CustomerBuilder.Make;

            Mapper.Map<CustomerDto>(model);
        }
    }
}
