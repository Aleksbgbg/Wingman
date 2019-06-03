namespace Wingman.ServiceFactory
{
    internal interface IConstructor
    {
        object Build(object[] arguments);
    }
}