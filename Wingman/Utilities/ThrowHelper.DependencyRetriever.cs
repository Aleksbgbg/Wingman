namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class DependencyRetriever
            {
                internal static void NoDefinitionForKey(string key)
                {
                    InvalidOperationException($"No service entry has been defined for the key '{key}', with a null service type.");
                }

                internal static void CannotSatisfyRequestFor(Type service, string key)
                {
                    InvalidOperationException($"No service entry has been defined for the service ({service.Name}) and key ({key}) combination, and the requested type is not a service factory function (Func<TService>) or an enumerable of services (IEnumerable<TService>).");
                }

                internal static void CannotSatisfyMultipleRequestFor(Type service)
                {
                    InvalidOperationException($"No service entries have been defined for the service '{service.Name}'.");
                }
            }
        }
    }
}