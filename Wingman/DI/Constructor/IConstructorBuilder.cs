namespace Wingman.DI.Constructor
{
    internal interface IConstructorBuilder
    {
        object BuildWith(object[] arguments);
    }
}