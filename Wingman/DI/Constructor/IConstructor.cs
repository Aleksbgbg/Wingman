namespace Wingman.DI.Constructor
{
    internal interface IConstructor : IConstructionInfo
    {
        bool AcceptsUserArguments(object[] userArguments);
    }
}