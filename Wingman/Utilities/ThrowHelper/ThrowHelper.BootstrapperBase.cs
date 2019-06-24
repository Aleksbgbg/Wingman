namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static class BootstrapperBase
        {
            internal static InvalidOperationException RootViewModelNotRegistered(Type rootViewModelType, string registrationMethod)
            {
                return InvalidOperationException($"Root ViewModel {rootViewModelType.Name} was not registered with the dependency registrar during {registrationMethod}.");
            }
        }
    }
}