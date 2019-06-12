# Wingman 
![Logo](/Wingman.png)

![NuGet Build](https://img.shields.io/nuget/v/Wingman.svg)

General-purpose C# assistance library.

Includes support for WPF with Caliburn.Micro.

## NuGet
Wingman is on [NuGet](https://www.nuget.org/packages/Wingman).

To install the latest version, open PowerShell in your project directory, and simply run:
```ps
Install-Package Wingman
```

## How to Use Wingman
Wingman currently includes basic support for Caliburn.Micro.

### BootstrapperBase
The author includes a `BootstrapperBase` base implementation which allows bootstrapping with a minimal amount of code.

The most basic bootstrapper might looks like this:
```cs
using Wingman.Bootstrapper;
using Wingman.Container;

class Bootstrapper : BootstrapperBase<IShellViewModel>
{
    protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
    {
        dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
    }
}
```

As you may have noticed, one of the `BootstrapperBase` classes accepts a single generic parameter - the root ViewModel to use for `DisplayRootViewModel`.

`RegisterViewModels` is an abstract method because the root ViewModel (`IShellViewModel` in this case) must always be registered with the dependency registrar, otherwise the root view cannot be displayed by Caliburn. The `BootstrapperBase` will throw if the root ViewModel isn't registered!

`BootstrapperBase` will also register two additional dependencies for you automatically:
- An `IWindowManager` implementation (from Caliburn)
  - This serves to spawn windows and is always necessary.
- An `IServiceFactory` implementation
  - This is a convenience general-purpose factory, provided by Wingman, described in its own section.
  
Caliburn's container bootstrapping methods have been overridden and sealed by Wingman, and they delegate all calls to `IDependencyRetriever`.

The `Configure` method has also been sealed, and calls these methods in order:
```cs
RegisterViewModels(IDependencyRegistrar dependencyRegistrar);
RegisterServices(IDependencyRegistrar dependencyRegistrar);

RegisterFactoryViewModels(IServiceFactoryRegistrar dependencyRegistrar);
RegisterFactoryServices(IServiceFactoryRegistrar dependencyRegistrar);
```

The first two methods are simply used to register dependencies with the default container. `TRootViewModel` must be registered in `RegisterViewModels`, however, apart from that, there is nothing to stop you from registering services in the first method, and ViewModels in the second. The convention was created to make registration easier to read, as there are usually two separate clusters of registrations in the `Configure` method.

The Factory versions of these methods relate to registering with `IServiceFactory`, which is described in its own section.

Wingman also overrides `OnStartup` using the "template method" pattern, and displays the root view for the ViewModel templated when inheriting `BootstrapperBase`.

You can override these two method to take action before or after displaying the root view, respectively:
```cs
OnStartupBeforeDisplayRootView(object sender, StartupEventArgs e);
OnStartupAfterDisplayRootView(object sender, StartupEventArgs e);
```

All other `BootstrapperBase` methods have been left intact as per Caliburn's implementations.

Wingman's `BootstrapperBase` implementation aims to reduce hassle when setting up a new project, ensuring that the app has been bootstrapped properly by throwing descriptive exceptions early on during the startup phase.

### DependencyContainer
As you may have noted, Wingman includes two dependency interfaces - `IDependencyRegistrar` and `IDependencyRetriever`. These serve to register and retrieve dependencies from a dependency container. The interfaces have largely been based on the [`SimpleContainer`](https://github.com/Caliburn-Micro/Caliburn.Micro/blob/master/src/Caliburn.Micro.Core/SimpleContainer.cs) implementation in Caliburn.

The interfaces are split due to the fact that registrations and retrieval from the dependency container often happen separately, which means that too much information is passed to consumers that do not use both concepts.

Wingman comes with a default implementation of these two interfaces (`DependencyContainer`), which can be instantiated via `DependencyContainerFactory.Create()`. `DependencyContainer` also implements `IDependencyActivator`, a simple event that is raised when an instance is created from the container, which isn't used by Wingman specifically, but is present in Caliburn's `SimpleContainer` implementation.

Any custom implementations of these interfaces can either manually implement `IDependencyRegistrar` and/or `IDependencyRetriever`, or derive the `DependencyContainerBase` class which implements both interfaces and the convenience generic methods defined in the interfaces. `IDependencyActivator` is optional.

For custom container implementations, you must pass instances of your registrar and retriever to the base constructor of `BootstrapperBase`:
```cs
using Wingman.Bootstrapper;
using Wingman.Container;

class Bootstrapper : BootstrapperBase<IShellViewModel>
{
    public Bootstrapper() : base(MyCustomContainerRegistrar.Instance, MyCustomContainerRetriever.Instance)
    {
    }

    protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
    {
        dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
    }
}
```

Custom implementations may be necessary when using different dependency containers, such as [Castle Windsor](https://github.com/castleproject/Windsor) and [Ninject](https://github.com/ninject/ninject).

Wingman may introduce custom implementations for these containers in the future. If you happen to implement one of these, feel free to send a pull request! (Include unit tests!)

### ServiceFactory
Wingman provides a general-purpose factory, which can generate implementations with dependencies as well as arguments. Unfortunately, to make the process simple, arguments are weakly-typed, passed as an `object[]`.

The ServiceFactory is split into two interfaces:
1. `IServiceFactoryRegistrar`
   - Registers dependency creation strategies with the factory.
2. `IServiceFactory`
   - Generates instances of registered services via the appropriate strategy.

The default implementations of these interfaces are `ServiceFactoryRegistrar` and `ServiceFactory` (respectively), and they can both be instantiated via `ServiceFactoryFactory.Create(IDependencyRegistrar, IDependencyRetriever)`. A pair of objects are returned, `IServiceFactoryRegistrar Registrar` and `IServiceFactory Factory`. The purpose of the registrar being a separate object is so that the registrar is allocated once at startup, all the dependencies are swiftly registered, and the registrar is garbage-collected, releasing all registration-related class instances.

The `BootstrapperBase` class already takes care of instantiating and registering the factory with the default dependency container.

Creating a service instance is as simple as this:
```cs
class MyService
{
    private readonly ISomeService _someService;

    public MyService(IServiceFactory serviceFactory)
    {
        _someService = serviceFactory.Create<ISomeService>(somePieceOfData);
    }
}
```

There are two strategies for registering services:
1. FromContainer
2. PerRequest

#### FromContainer
The FromContainer strategy will simply retrieve the service from the container, letting the container do any dependency resolution. This means that you cannot pass any arguments to the service being created (an exception will throw!).

FromContainer dependencies only need an interface registration with the factory, and must be registered with the dependency container beforehand:
```cs
using Wingman.Bootstrapper;
using Wingman.Container;
using Wingman.ServiceFactory;

class Bootstrapper : BootstrapperBase<IShellViewModel>
{
    protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
    {
        dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
    }
    
    protected override void RegisterServices(IDependencyRegistrar dependencyRegistrar)
    {
        // Any registration strategy applies here, Singleton is just used by default
        dependencyRegistrar.Singleton<IService, ServiceImpl>();
    }
    
    protected override void RegisterFactoryServices(IServiceFactoryRegistrar serviceFactoryRegistrar)
    {
        serviceFactoryRegistrar.FromContainer<IService>();
    }
}
```

The default `IServiceFactoryRegistrar` will throw if the `FromContainer` service hasn't been registered with the `IDependencyRegistrar`!

The above registration means a use like this applies:
```cs
serviceFactory.Create<IService>();
```

However, this will throw:
```cs
serviceFactory.Create<IService>(argument1, argument2); // Cannot pass arguments to FromContainer registrations
```

#### PerRequest
The PerRequest strategy will resolve all dependencies, and then inject constructor parameters.

Take the following service:
```cs
class ServiceImpl : IService
{
    public ServiceImpl(IDependency dependency, IDependency2 dependency2, string myName)
    {
        // blah
    }
}
```

You can create it like this:
```cs
serviceFactory.Create<IService>("You are called ServiceImpl.");
```
The dependencies will be resolved automagically.

The above usage implies this kind of registration:
```cs
using Wingman.Bootstrapper;
using Wingman.Container;
using Wingman.ServiceFactory;

class Bootstrapper : BootstrapperBase<IShellViewModel>
{
    protected override void RegisterViewModels(IDependencyRegistrar dependencyRegistrar)
    {
        dependencyRegistrar.Singleton<IShellViewModel, ShellViewModel>();
    }
    
    protected override void RegisterServices(IDependencyRegistrar dependencyRegistrar)
    {
        // Any registration strategy applies here, Singleton is just used by default
        dependencyRegistrar.Singleton<IDependency, DependencyImpl>();
        dependencyRegistrar.Singleton<IDependency2, Dependency2Impl>();
    }
    
    protected override void RegisterFactoryServices(IServiceFactoryRegistrar serviceFactoryRegistrar)
    {
        serviceFactoryRegistrar.PerRequest<IService, ServiceImpl>();
    }
}
```

#### Alternative Use
ServiceFactory can be used outside of WPF and Caliburn.Micro projects, however the default implementation depends on `IDependencyRetriever`, which may be a slightly restrictive interface, due to its Caliburn-oriented nature.

Should you wish to use the ServiceFactory in other contexts, it will still work equally as well, however you might need custom implementations of `IDependencyRetriever` for your specific dependency container (see the **DependencyContainer** section).

Feel free to post pull requests or issues of such usages, informing the author of ways the library can be shaped to facilitate ease of such use.