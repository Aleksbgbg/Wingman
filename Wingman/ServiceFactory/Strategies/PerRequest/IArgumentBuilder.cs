namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IArgumentBuilder
    {
        object[] BuildArguments();
    }
}