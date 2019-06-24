namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static class ServiceFactory
        {
            internal static InvalidOperationException NoHandlerRegisteredWithContainer(Type interfaceType)
            {
                return InvalidOperationException($"Handler for {interfaceType.Name} has not been registered with the dependency container.");
            }

            internal static InvalidOperationException DuplicateRegistration(Type interfaceType)
            {
                return InvalidOperationException($"{interfaceType.Name} has already been registered with the {nameof(ServiceFactory)}.");
            }

            internal static InvalidOperationException NoDependencyMapping(Type interfaceType)
            {
                return InvalidOperationException($"No dependency mapping exists for {interfaceType.Name}. Register the interface first.");
            }

            internal static InvalidOperationException RegisterNonConcreteTypePerRequest(Type concreteType)
            {
                return InvalidOperationException($"Cannot register {concreteType.Name} as it is not a concrete type.");
            }
        }
    }
}