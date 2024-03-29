﻿# laget.Mapper
An extremely simple object-object mapper.

![Nuget](https://img.shields.io/nuget/v/laget.Mapper)
![Nuget](https://img.shields.io/nuget/dt/laget.Mapper)

## Configuration
> This example is shown using Autofac since this is the go-to IoC for us.

```c#
await Host.CreateDefaultBuilder()
    .ConfigureContainer<ContainerBuilder>((context, builder) =>
    {
        builder.RegisterMappers();
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build()
    .RunAsync();
```

This is the simplest way to register your mappers but also has some constraints:
* The mapper implementations must exist in the program that calls `RegisterMappers`.
* The mapper implementations must implement the interface `IMapper`.


```c#
await Host.CreateDefaultBuilder()
    .ConfigureContainer<ContainerBuilder>((context, builder) =>
    {
        builder.RegisterMappers(_ =>
        {
            _.TheCallingAssembly();
            _.TheCallingAssembly<T>();
            _.AssemblyContainingType<T>();
            _.Assembly("Mappers");
        });
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build()
    .RunAsync();
```

This is the advanced and more customizable way to register your mappers

* `TheCallingAssembly();` will register mapper implementations from the calling assembly that implements the interface `IMapper`.
* `TheCallingAssembly<T>();` will register mapper implementations from the calling assembly that implements the interface `T`.
* `AssemblyContainingType<T>();` will register mapper implementations from the assembly of the provided type, this is useful if you e.g. have class libraries with Mappers and/or custom implementations of `IMapper`.
* `Assembly("name");` will register mapper implementations in the assembly, will load assembly via the name provided using `System.Reflection`, that implements the interface `IMapper`.

## Usage
### Creating a mapper class
To create a mapper class use the marker interface `IMapper` to mark the class and the `MapperMethod` attribute to mark all methods in the class that are mapping methods.

The global mapper will pick up all classes tagged with the `IMapper` marker interface and register all methods tagged with the `MapperMethod` that match the following critera
 - The method is not private
 - The method has a non void return value 
 - The method has only one parameter

```c#
public class ModelMapper : IMapper 
{
    [MapperMethod] // Will be registered as the mapping method for converting Model -> Entity
    public Entity ModelToEntity(Model model) => 
        new Entity 
        {
            // ...
        };
        
    [MapperMethod] // Will be registered as the mapping method for converting Dto -> Model
    public Model ModelFromDto(Dto dto) => 
        new Model 
        {
            // ...
        };
        
    [MapperMethod] // Will not be registered as the mapping method for converting Dto -> Model is already defined above
    public Model ModelFromDto2(Dto dto) => 
        new Model 
        {
            // ...
        };
        
    // Will not be registered as it lacks the MapperMethod attribute
    public Dto DtoFromModel(Model model) =>
        new Dto 
        {
            // ...
        };
    
    [MapperMethod] // Will not be registered as it is a private method
    private Model ModelFromEntity(Entity entity) =>
        new Model 
        {
            // ...
        };
        
    [MapperMethod] // Will not be registered as it returns void
    public void UpdateModelState(Model model) { /* ... */ }
     
    [MapperMethod] // Will not be registered as it doesn't take a parameter
    public int GetConstantInt() => 42;
        
    [MapperMethod] // Will not be registered as it takes multiple arguments
    public Model ModelFromEntityAndDto(Entity entity, Dto dto) =>
        new Model 
        {
            // ...
        };
}
```

### Using the mapper
The mapper can be used in different ways, either through direct access to the static method

```c#
var model = new Model();
var dto = Mapper.Mapper.Map<Dto>(model);
var baseClassDto = Mapper.Mapper.Map<ModelBase, DtoBase>(model);
```

Or using one of the built in extensions on `object` or `IEnumerable<TSource>`
```c#
var model = new Model();
var dto = model.Map<Dto>();

var dtos = new [] { new Dto() /*, ... */ };
var models = dtos.Map<Model>();
```

Using the extensions is the recommended way as it provides a clean way of writing Linq chains, however it can be useful to access the mapper directly if you need to map based on an inherited class.

## Benchmarks
```ini
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4123/22H2/2022Update) (VMware)
AMD Ryzen Threadripper 3960X, 1 CPU, 12 logical and 12 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 6.0.27 (6.0.2724.6912), X64 RyuJIT AVX2
  Job-BJZLVN : .NET 6.0.27 (6.0.2724.6912), X64 RyuJIT AVX2
  Job-OQIHXW : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

IterationCount=10  LaunchCount=1  RunStrategy=Throughput  WarmupCount=5
```

### Mappings
| Method          | Runtime  | Mean      | Error     | StdDev    | Min       | Max       | Median    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------- |--------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Simple_Mapper   | .NET 6.0 | 169.49 ns |  3.408 ns |  2.254 ns | 166.39 ns | 172.78 ns | 168.89 ns |  1.00 |    0.00 | 0.0172 |     144 B |        1.00 |
| Simple_Mapper   | .NET 8.0 |  63.00 ns |  4.291 ns |  2.838 ns |  59.19 ns |  67.89 ns |  62.52 ns |  0.37 |    0.02 | 0.0172 |     144 B |        1.00 |
|                 |          |           |           |           |           |           |           |       |         |        |           |             |
| DeepType_Mapper | .NET 6.0 | 976.40 ns |  9.541 ns |  5.678 ns | 971.22 ns | 986.86 ns | 975.08 ns |  1.00 |    0.00 | 0.1411 |    1184 B |        1.00 |
| DeepType_Mapper | .NET 8.0 | 461.82 ns | 27.469 ns | 18.169 ns | 433.24 ns | 488.14 ns | 466.44 ns |  0.48 |    0.02 | 0.1411 |    1184 B |        1.00 |

### Registration
| Method                              | Mean        | Error     | StdDev    | Min         | Max         | Median      | Gen0   | Allocated |
|------------------------------------ |------------:|----------:|----------:|------------:|------------:|------------:|-------:|----------:|
| Single_Registration                 |    47.44 ns |  5.158 ns | 15.127 ns |    46.44 ns |    21.13 ns | 1,989.74 ns | 0.0839 |     720 B |
| Multiple_Registration               |    72.98 ns | 13.068 ns | 38.532 ns |    75.02 ns |    85.02 ns | 4,160.33 ns | 0.1831 |    1568 B |
| Assembly_Registration               |    52.25 ns |  2.468 ns |  7.160 ns |    52.30 ns |    36.57 ns | 1,076.43 ns | 0.0515 |     432 B |
| AssemblyContainingType_Registration |    47.87 ns |  1.102 ns |  3.248 ns |    47.74 ns |    43.13 ns |    53.77 ns | 0.0334 |     280 B |
| TheCallingAssembly_Registration     |    54.38 ns |  0.764 ns |  2.254 ns |    53.33 ns |    50.01 ns |    57.88 ns | 0.0334 |     280 B |
| TheCallingAssemblyT_Registration    |    54.31 ns |  1.045 ns |  3.082 ns |    54.44 ns |    48.48 ns |    59.92 ns | 0.0334 |     280 B |
| TheEntryAssembly_Registration       |    50.83 ns |  1.015 ns |  2.993 ns |    49.76 ns |    47.22 ns |    59.54 ns | 0.0334 |     280 B |
| TheEntryAssemblyT_Registration      |    49.96 ns |  0.665 ns |  1.961 ns |    48.95 ns |    47.30 ns |    52.91 ns | 0.0334 |     280 B |
| TheExecutingAssembly_Registration   |   657.93 ns |  1.320 ns |  3.831 ns |   657.71 ns |   649.10 ns |   667.97 ns | 0.0334 |     280 B |
| TheExecutingAssemblyT_Registration  |   699.09 ns |  2.773 ns |  8.089 ns |   699.08 ns |   684.46 ns |   717.44 ns | 0.0334 |     280 B |
