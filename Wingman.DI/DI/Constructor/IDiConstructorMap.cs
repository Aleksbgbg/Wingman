namespace Wingman.DI.Constructor
{
    internal interface IDiConstructorMap
    {
        IConstructionInfo FindBestConstructorForDi();
    }
}