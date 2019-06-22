namespace Wingman.DI.Constructor
{
    using System;

    internal interface IConstructor : IConstructorBuilder
    {
        int ParameterCount { get; }

        Type ParameterTypeAt(int index);

        bool AcceptsUserArguments(object[] userArguments);
    }
}