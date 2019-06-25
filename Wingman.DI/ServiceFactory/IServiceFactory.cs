namespace Wingman.ServiceFactory
{
    /// <summary> A general-purpose service factory for types registered with a <see cref="IServiceFactoryRegistrar"/>, which allows passing parameters to the constructor. </summary>
    public interface IServiceFactory
    {
        /// <summary> Creates the associated concrete type of <typeparamref name="TService"/>, via the appropriate retrieval strategy. </summary>
        TService Create<TService>(params object[] arguments);
    }
}