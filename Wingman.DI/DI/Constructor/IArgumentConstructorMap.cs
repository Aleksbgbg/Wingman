namespace Wingman.DI.Constructor
{
    internal interface IArgumentConstructorMap
    {
        IConstructionInfo FindBestConstructorForArguments(object[] arguments);
    }
}