# laget.Mapper
An extremely simple object-object mapper.

![Nuget](https://img.shields.io/nuget/v/laget.Mapper)
![Nuget](https://img.shields.io/nuget/dt/laget.Mapper)

## Configuration
> This example is shown using Autofac since this is the go-to IoC for us.

## Usage
```c#
await Host.CreateDefaultBuilder()
    .ConfigureContainer<ContainerBuilder>((context, builder) =>
    {
        builder.RegisterMaps();
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .Build()
    .RunAsync();
```
