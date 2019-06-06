namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    using System;

    internal interface IConstructor
    {
        int ArgumentCount { get; }

        Type ParameterTypeAt(int index);

        bool AcceptsUserArguments(object[] userArguments);

        object Build(object[] fullArguments);
    }
}