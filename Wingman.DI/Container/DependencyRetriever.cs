﻿namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Wingman.Container.Entries;
    using Wingman.Container.Strategies;
    using Wingman.Utilities.ThrowHelper;

    /// <summary> Default implementation of <see cref="IDependencyRetriever"/> and <see cref="IDependencyActivator"/>. </summary>
    public class DependencyRetriever : DependencyRetrieverBase, IDependencyActivator
    {
        private readonly IServiceEntryStore _serviceEntryStore;

        internal DependencyRetriever(IServiceEntryStore serviceEntryStore)
        {
            _serviceEntryStore = serviceEntryStore;
        }

        public event Action<object> Activated;

        public override object GetInstance(Type service, string key = null)
        {
            ServiceEntry serviceEntry = CreateServiceEntry(service, key);

            if (DefinitionExistsInStoreFor(serviceEntry))
            {
                return LocateServiceFor(serviceEntry);
            }

            if (service == null)
            {
                throw ThrowHelper.DependencyRetriever.NoDefinitionForKey(key);
            }

            if (IsFactoryFuncType(service))
            {
                return CreateFactoryFunc(service, key);
            }

            if (IsEnumerableType(service))
            {
                return CreateEnumerable(service);
            }

            throw ThrowHelper.DependencyRetriever.CannotSatisfyRequestFor(service, key);
        }

        public override IEnumerable<object> GetAllInstances(Type service)
        {
            ServiceEntry serviceEntry = CreateServiceEntry(service);

            if (DefinitionExistsInStoreFor(serviceEntry))
            {
                return LocateAllServicesFor(serviceEntry);
            }

            throw ThrowHelper.DependencyRetriever.CannotSatisfyMultipleRequestFor(service);
        }

        public override void BuildUp(object instance)
        {
            foreach (PropertyInfo property in FindInjectableProperties(instance))
            {
                object[] injectionInstances = GetAllInstances(property.PropertyType).ToArray();

                if (injectionInstances.Length != 0)
                {
                    property.SetValue(instance, injectionInstances.First(), index: null);
                }
            }
        }

        private static ServiceEntry CreateServiceEntry(Type service, string key)
        {
            return new ServiceEntry(service, key);
        }

        private static ServiceEntry CreateServiceEntry(Type service)
        {
            return new ServiceEntry(service, null);
        }

        private bool DefinitionExistsInStoreFor(ServiceEntry serviceEntry)
        {
            return _serviceEntryStore.HasHandler(serviceEntry);
        }

        private object LocateServiceFor(ServiceEntry serviceEntry)
        {
            return LocateService(RetrieveHandlers(serviceEntry).Single());
        }

        private IEnumerable<object> LocateAllServicesFor(ServiceEntry serviceEntry)
        {
            return RetrieveHandlers(serviceEntry).Select(LocateService);
        }

        private object LocateService(IServiceLocationStrategy serviceLocationStrategy)
        {
            object service = serviceLocationStrategy.LocateService();

            Activated?.Invoke(service);

            return service;
        }

        private IEnumerable<IServiceLocationStrategy> RetrieveHandlers(ServiceEntry serviceEntry)
        {
            return _serviceEntryStore.RetrieveHandlers(serviceEntry);
        }

        private static bool IsFactoryFuncType(Type service)
        {
            return MatchesGenericTypeDefinition(service, typeof(Func<>));
        }

        private static bool IsEnumerableType(Type service)
        {
            return MatchesGenericTypeDefinition(service, typeof(IEnumerable<>));
        }

        private static bool MatchesGenericTypeDefinition(Type type, Type genericTypeDefinition)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == genericTypeDefinition;
        }

        private object CreateFactoryFunc(Type factoryFuncType, string key)
        {
            Type retrieverType = GetType();
            Type factoryObjectType = factoryFuncType.GetGenericArguments()[0];
            MethodInfo factoryGeneratorMethod = retrieverType.GetMethod(nameof(CreateFactoryFuncForType),
                                                                        BindingFlags.Instance | BindingFlags.NonPublic
                                                              )
                                                             .MakeGenericMethod(factoryObjectType);

            object factoryFunc = factoryGeneratorMethod.Invoke(this, new object[] { key });

            return factoryFunc;
        }

        private Func<T> CreateFactoryFuncForType<T>(string key)
        {
            return () => GetInstance<T>(key);
        }

        private object CreateEnumerable(Type service)
        {
            Type objectType = service.GetGenericArguments()[0];

            return GetAllInstances(objectType).ToArray();
        }

        private IEnumerable<PropertyInfo> FindInjectableProperties(object instance)
        {
            return instance.GetType()
                           .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                           .Where(PropertyCanBeInjectedInto);
        }

        private bool PropertyCanBeInjectedInto(PropertyInfo property)
        {
            return property.CanWrite &&
                   DefinitionExistsInStoreFor(CreateServiceEntry(property.PropertyType));
        }
    }
}