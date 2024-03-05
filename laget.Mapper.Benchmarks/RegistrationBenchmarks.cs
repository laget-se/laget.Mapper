using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using laget.Mapper.Benchmarks.Mappers;
using laget.Mapper.Core;
using System.Collections.Generic;

namespace laget.Mapper.Benchmarks
{
    [MemoryDiagnoser]
    [MeanColumn, MinColumn, MaxColumn, MedianColumn]
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.Net80, 2, 10, 50)]
    public class RegistrationBenchmarks
    {
        [Benchmark]
        public void Benchmark_Single_Registration()
        {
            Mapper.Reset();
            Mapper.RegisterMapper(new AddressMapper());
        }

        [Benchmark]
        public void Benchmark_Multiple_Registration()
        {
            Mapper.Reset();
            Mapper.RegisterMappers(new List<IMapper> { new CustomerMapper(), new AddressMapper() });
        }

        [Benchmark]
        public void Benchmark_Assembly_Registration()
        {
            var registrator = new Registrator(null);
            registrator.Assembly("laget.Mapper.Benchmarks");
        }

        [Benchmark]
        public void Benchmark_AssemblyContainingType_Registration()
        {
            var registrator = new Registrator(null);
            registrator.AssemblyContainingType<IMapper>();
        }

        [Benchmark]
        public void Benchmark_TheCallingAssembly_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheCallingAssembly();
        }

        [Benchmark]
        public void Benchmark_TheCallingAssemblyT_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheCallingAssembly<IMapper>();
        }

        [Benchmark]
        public void Benchmark_TheEntryAssembly_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheEntryAssembly();
        }

        [Benchmark]
        public void Benchmark_TheEntryAssemblyT_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheEntryAssembly<IMapper>();
        }

        [Benchmark]
        public void Benchmark_TheExecutingAssembly_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheExecutingAssembly();
        }

        [Benchmark]
        public void Benchmark_TheExecutingAssemblyT_Registration()
        {
            var registrator = new Registrator(null);
            registrator.TheExecutingAssembly<IMapper>();
        }
    }
}
