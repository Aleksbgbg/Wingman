namespace Wingman.DI
{
    using System;

    internal interface IConstructorCandidateEvaluator
    {
        IConstructor FindBestConstructorForDi(Type implementation);
    }
}