namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IArgumentBuilder
    {
        object[] BuildArgumentsForConstructor(IConstructor constructor, object[] userArguments);
    }
}