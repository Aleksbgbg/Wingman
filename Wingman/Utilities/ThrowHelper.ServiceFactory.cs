namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class ServiceFactory
            {
                internal static void NoHandlerRegisteredWithContainer(Type interfaceType)
                {
                    InvalidOperationException($"Handler for {interfaceType.Name} has not been registered with the dependency container.");
                }

                internal static void DuplicateRegistration(Type interfaceType)
                {
                    InvalidOperationException($"{interfaceType.Name} has already been registered with the {nameof(ServiceFactory)}.");
                }

                internal static void NoDependencyMapping(Type interfaceType)
                {
                    InvalidOperationException($"No dependency mapping exists for {interfaceType.Name}. Register the interface first.");
                }
            }
        }
    }
}