﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using laget.Mapper.Benchmarks.Builders;
using laget.Mapper.Benchmarks.Mappers;
using laget.Mapper.Benchmarks.Models;

namespace laget.Mapper.Benchmarks
{
    [MemoryDiagnoser]
    [MeanColumn, MinColumn, MaxColumn, MedianColumn]
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.Net60, 1, 5, 10, baseline: true)]
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.Net80, 1, 5, 10)]
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
