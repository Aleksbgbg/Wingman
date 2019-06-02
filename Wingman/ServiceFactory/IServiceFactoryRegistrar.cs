namespace Wingman.ServiceFactory
{
    using Wingman.Container;

    /// <summary> A registrar which associates service interfaces with their implementations, that can be later retrieved through a <see cref="IServiceFactory"/>. </summary>
    public interface IServiceFactoryRegistrar
    {
        /// <summary> The <see cref="IServiceFactory"/> will request an instance of <typeparamref name="TService"/> from the <see cref="IDependencyRetriever"/> for each request. </summary>
        void RegisterFromRetriever<TService>();

        /// <summary> The <see cref="IServiceFactory"/> will create a new instance of the <typeparamref name="TImplementation"/> for each request of <typeparamref name="TService"/>. </summary>
        void RegisterPerRequest<TService, TImplementation>() where TImplementation : TService;
    }
}