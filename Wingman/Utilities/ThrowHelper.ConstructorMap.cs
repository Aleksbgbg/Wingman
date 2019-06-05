namespace Wingman.Utilities
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class ConstructorMap
            {
                internal static void NoPublicInstanceConstructors(Type concreteType)
                {
                    InvalidOperationException($"There are no public instance constructors for {concreteType.Name}.");
                }
            }
        }
    }
}