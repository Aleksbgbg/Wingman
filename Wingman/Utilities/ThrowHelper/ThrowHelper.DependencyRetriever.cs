namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static class DependencyRetriever
        {
            internal static InvalidOperationException NoDefinitionForKey(string key)
            {
                return InvalidOperationException($"No service entry has been defined for the key '{key}', with a null service type.");
            }

            internal static InvalidOperationException CannotSatisfyRequestFor(Type service, string key)
            {
                return InvalidOperationException($"No service entry has been defined for the service ({service.Name}) and key ({key}) combination, and the requested type is not a service factory function (Func<TService>) or an enumerable of services (IEnumerable<TService>).");
            }

            internal static InvalidOperationException CannotSatisfyMultipleRequestFor(Type service)
            {
                return InvalidOperationException($"No service entries have been defined for the service '{service.Name}'.");
            }
        }
    }
}