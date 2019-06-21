namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class ConstructorCandidateEvaluator
            {
                internal static void NoPublicInstanceConstructors(Type implementation)
                {
                    InvalidOperationException($"There are no public instance constructors for {implementation.Name}.");
                }
            }
        }
    }
}