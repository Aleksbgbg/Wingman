namespace Wingman.DI.Constructor
{
    internal interface IConstructorMap
    {
        IConstructor FindBestFitForArguments(object[] arguments);
    }
}