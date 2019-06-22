namespace Wingman.DI.Constructor
{
    internal interface IConstructor : IConstructorParameterInfo, IConstructorBuilder
    {
        bool AcceptsUserArguments(object[] userArguments);
    }
}