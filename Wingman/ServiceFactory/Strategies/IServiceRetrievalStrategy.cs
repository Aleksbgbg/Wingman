namespace Wingman.ServiceFactory.Strategies
{
    internal interface IServiceRetrievalStrategy
    {
        object RetrieveService(object[] arguments);
    }
}