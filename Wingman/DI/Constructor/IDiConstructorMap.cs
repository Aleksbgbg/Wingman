namespace Wingman.DI.Constructor
{
    internal interface IDiConstructorMap
    {
        IConstructor FindBestConstructorForDi();
    }
}