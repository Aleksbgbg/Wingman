namespace Wingman.ServiceFactory
{
    internal interface IArgumentBuilder
    {
        object[] BuildArgumentsForConstructor(IConstructor constructor, object[] userArguments);
    }
}