namespace Wingman.ServiceFactory.Strategies.PerRequest
{
    internal interface IConstructor
    {
        object Build(object[] arguments);
    }
}