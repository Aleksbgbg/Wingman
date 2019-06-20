namespace Wingman.DI
{
    using System;

    internal interface IConstructor
    {
        int ParameterCount { get; }

        Type ParameterTypeAt(int index);

        bool AcceptsUserArguments(object[] userArguments);

        object Build(object[] fullArguments);
    }
}