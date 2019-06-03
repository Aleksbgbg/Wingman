namespace Wingman.Utilities
{
    internal static partial class ThrowHelper
    {
        internal static partial class Throw
        {
            internal static class FromRetrieverRetrievalStrategy
            {
                internal static void ArgumentsNotEmpty(string paramName, object actualValue)
                {
                    ArgumentOutOfRangeException(paramName, actualValue, "Cannot pass arguments to a 'FromRetriever' dependency mapping.");
                }
            }
        }
    }
}