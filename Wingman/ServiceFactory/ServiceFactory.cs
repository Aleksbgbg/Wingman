namespace Wingman.ServiceFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Wingman.Container;
    using Wingman.Utilities;

    /// <summary> Default implementation of <see cref="IServiceFactory"/>. </summary>
    public class ServiceFactory : IServiceFactory, IServiceFactoryRegistrar
    {
        private readonly IDependencyRegistrar _dependencyRegistrar;

        private readonly IDependencyRetriever _dependencyRetriever;

        private readonly Dictionary<Type, Type> _interfaceToConcreteType = new Dictionary<Type, Type>();

        public ServiceFactory(IDependencyRegistrar dependencyRegistrar, IDependencyRetriever dependencyRetriever)
        {
            _dependencyRegistrar = dependencyRegistrar;
            _dependencyRetriever = dependencyRetriever;
        }

        /// <inheritdoc/>
        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            Register(typeof(TService), typeof(TImplementation));
        }

        /// <inheritdoc/>
        public TService Make<TService>(params object[] arguments)
        {
            return (TService)Make(typeof(TService), arguments);
        }

        private void Register(Type interfaceType, Type concreteType)
        {
            if (!_dependencyRegistrar.HasHandler(interfaceType, key: null))
            {
                ThrowHelper.Throw.ServiceFactory.NoHandlerRegisteredWithContainer(interfaceType);
            }

            if (_interfaceToConcreteType.ContainsKey(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.DuplicateRegistration(interfaceType);
            }

            if (concreteType.IsAbstract)
            {
                ThrowHelper.Throw.ServiceFactory.CannotRegisterConcreteType(concreteType);
            }

            _interfaceToConcreteType[interfaceType] = concreteType;
        }

        private object Make(Type interfaceType, object[] arguments)
        {
            if (!_interfaceToConcreteType.ContainsKey(interfaceType))
            {
                ThrowHelper.Throw.ServiceFactory.NoDependencyMapping(interfaceType);
            }

            Type concreteType = _interfaceToConcreteType[interfaceType];

            ServiceConstructor[] serviceConstructors = concreteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                                                   .Select(constructorInfo => new ServiceConstructor(constructorInfo, arguments))
                                                                   .Where(constructor => constructor.AcceptsUserArguments())
                                                                   .ToArray();

            if (serviceConstructors.Length == 0)
            {
                ThrowHelper.Throw.ServiceFactory.NoMatchingConstructors(interfaceType);
            }

            if (serviceConstructors.Length > 1)
            {
                ThrowHelper.Throw.ServiceFactory.TooManyConstructors(interfaceType);
            }

            ServiceConstructor targetServiceConstructor = serviceConstructors[0];

            return new TypeFactory(_dependencyRetriever, targetServiceConstructor).MakeType();
        }
    }
}