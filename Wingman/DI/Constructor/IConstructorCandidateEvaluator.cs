namespace Wingman.DI.Constructor
{
    using System;

    internal interface IConstructorCandidateEvaluator
    {
        IConstructor FindBestConstructorForDi(Type implementation);
    }
}