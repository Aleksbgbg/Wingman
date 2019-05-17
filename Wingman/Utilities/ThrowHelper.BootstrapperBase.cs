namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class BootstrapperBase
            {
                internal static void RootViewModelNotRegistered(Type rootViewModelType, string registrationMethod)
                {
                    InvalidOperationException($"Root ViewModel {rootViewModelType.Name} was not registered with the dependency registrar during {registrationMethod}.");
                }
            }
        }
    }
}