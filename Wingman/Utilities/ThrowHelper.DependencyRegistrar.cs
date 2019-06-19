namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class DependencyRegistrar
            {
                internal static void DuplicateRegistration(Type interfaceType)
                {
                    InvalidOperationException($"{interfaceType.Name} has already been registered with the {nameof(DependencyRegistrar)}.");
                }
            }
        }
    }
}