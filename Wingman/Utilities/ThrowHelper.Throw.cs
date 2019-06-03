namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            private static void InvalidOperationException(string message)
            {
                throw new InvalidOperationException(message);
            }

            private static void ArgumentOutOfRangeException(string paramName, object actualValue, string message)
            {
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }
    }
}