namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        private static InvalidOperationException InvalidOperationException(string message)
        {
            return new InvalidOperationException(message);
        }
    }
}