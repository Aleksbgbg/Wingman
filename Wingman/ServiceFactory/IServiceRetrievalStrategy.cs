namespace Wingman.ServiceFactory
{
    internal interface IServiceRetrievalStrategy
    {
        object RetrieveService(object[] arguments);
    }
}