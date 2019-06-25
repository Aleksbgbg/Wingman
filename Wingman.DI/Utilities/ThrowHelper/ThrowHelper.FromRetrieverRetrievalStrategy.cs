namespace Wingman.Utilities.ThrowHelper
{
    using System;

    internal static partial class ThrowHelper
    {
        internal static class FromRetrieverRetrievalStrategy
        {
            internal static ArgumentOutOfRangeException ArgumentsNotEmpty(string paramName, object actualValue)
            {
                return ArgumentOutOfRangeException(paramName, actualValue, "Cannot pass arguments to a 'FromRetriever' dependency mapping.");
            }
        }
    }
}