namespace Wingman.DI.Constructor
{
    internal interface IArgumentConstructorMap
    {
        IConstructor FindBestConstructorForArguments(object[] arguments);
    }
}