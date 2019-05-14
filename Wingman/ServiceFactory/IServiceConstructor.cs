namespace Wingman.ServiceFactory
{
    using System;

    internal interface IServiceConstructor
    {
        int DependencyCount { get; }

        int ArgumentCount { get; }

        object[] UserArguments { get; }

        bool AcceptsUserArguments();

        bool HasDependencies();

        Type ArgumentTypeAtIndex(int index);

        object CreateObjectUsingArguments(object[] arguments);
    }
}