namespace Wingman.DI.Constructor
{
    internal interface IConstructorInvoker
    {
        object InvokeConstructor(object[] arguments);
    }
}