namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary> An implementation adapter for the default Caliburn.Micro IoC container. </summary>
    public class DependencyContainer : DependencyContainerBase, IDependencyActivator
    {
        private readonly ISimpleContainer _simpleContainer;

        internal DependencyContainer(ISimpleContainer simpleContainer)
        {
            _simpleContainer = simpleContainer;
        }

        public event Action<object> Activated
        {
            add => _simpleContainer.Activated += value;

            remove => _simpleContainer.Activated -= value;
        }

        public override void RegisterInstance(Type service, object implementation, string key = null)
        {
            _simpleContainer.RegisterInstance(service, key, implementation);
        }

        public override void RegisterPerRequest(Type service, Type implementation, string key = null)
        {
            _simpleContainer.RegisterPerRequest(service, key, implementation);
        }

        public override void RegisterSingleton(Type service, Type implementation, string key = null)
        {
            _simpleContainer.RegisterSingleton(service, key, implementation);
        }

        public override void RegisterHandler(Type service, Func<IDependencyRetriever, object> handler, string key = null)
        {
            _simpleContainer.RegisterHandler(service, key, _ => handler(this));
        }

        public override void UnregisterHandler(Type service, string key = null)
        {
            _simpleContainer.UnregisterHandler(service, key);
        }

        public override bool HasHandler(Type service, string key = null)
        {
            return _simpleContainer.HasHandler(service, key);
        }

        public override object GetInstance(Type service, string key = null)
        {
            return _simpleContainer.GetInstance(service, key);
        }

        public override IEnumerable<object> GetAllInstances(Type service)
        {
            return _simpleContainer.GetAllInstances(service);
        }

        public override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }
    }
}