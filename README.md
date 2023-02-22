# laget.Mapper
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
