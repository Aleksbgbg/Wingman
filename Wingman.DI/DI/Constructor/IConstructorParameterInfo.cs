namespace Wingman.DI.Constructor
{
    using System;

    internal interface IConstructorParameterInfo
    {
        int ParameterCount { get; }

        Type ParameterTypeAt(int index);
    }
}