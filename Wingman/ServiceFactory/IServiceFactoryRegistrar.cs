namespace Wingman.ServiceFactory
{
    /// <summary> A registrar which associates service interfaces with their implementations, that can be later retrieved through the default service factory. </summary>
    public interface IServiceFactoryRegistrar
    {
        /// <summary> Associates the <typeparamref name="TService"/> with the <typeparamref name="TImplementation"/> in the default service factory. </summary>
        void Register<TService, TImplementation>() where TImplementation : TService;
    }
}