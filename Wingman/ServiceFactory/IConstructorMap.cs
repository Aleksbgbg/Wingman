namespace Wingman.ServiceFactory
{
    internal interface IConstructorMap
    {
        IConstructor FindBestFitForArguments(object[] arguments);
    }
}