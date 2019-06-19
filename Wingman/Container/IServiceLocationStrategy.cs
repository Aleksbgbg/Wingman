namespace Wingman.Container
{
    internal interface IServiceLocationStrategy
    {
        object LocateService(IDependencyRetriever dependencyRetriever);
    }
}