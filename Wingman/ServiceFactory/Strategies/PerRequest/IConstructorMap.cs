namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IConstructorMap
    {
        IConstructor FindBestFitForArguments(object[] arguments);
    }
}