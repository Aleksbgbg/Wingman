namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IArgumentBuilderFactory
    {
        IArgumentBuilder CreateBuilderFor(IConstructor constructor, object[] userArguments);
    }
}