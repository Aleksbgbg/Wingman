namespace Wingman.ServiceFactory
{
    /// <summary> A service factory which allows passing parameters to the constructor. </summary>
    public interface IServiceFactory
    {
        /// <summary> Creates the associated concrete type of <typeparamref name="TService"/>, passing the arguments into its constructor. </summary>
        TService Make<TService>(params object[] arguments);
    }
}