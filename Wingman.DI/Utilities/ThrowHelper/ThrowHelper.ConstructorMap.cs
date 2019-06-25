namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static class ConstructorMap
        {
            internal static InvalidOperationException NoPublicInstanceConstructors(Type concreteType)
            {
                return InvalidOperationException($"There are no public instance constructors for {concreteType.Name}.");
            }
        }
    }
}