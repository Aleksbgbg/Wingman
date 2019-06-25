namespace Wingman.Container
{
    using System;

    /// <summary> Dependency manager that raises <see cref="Activated"/> when a new dependency instance is created from a <see cref="IDependencyRetriever"/>. </summary>
    public interface IDependencyActivator
    {
        /// <summary> Occurs when a new dependency instance is created. </summary>
        event Action<object> Activated;
    }
}