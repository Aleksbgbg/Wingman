namespace Wingman.DI
{
    internal interface IConstructorMap
    {
        IConstructor FindBestFitForArguments(object[] arguments);
    }
}