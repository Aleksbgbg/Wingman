namespace Wingman.Container
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    internal interface ISimpleContainer
    {
        event Action<object> Activated;

        void RegisterInstance(Type service, string key, object implementation);

        void RegisterPerRequest(Type service, string key, Type implementation);

        void RegisterSingleton(Type service, string key, Type implementation);

        void RegisterHandler(Type service, string key, Func<SimpleContainer, object> handler);

        void UnregisterHandler(Type service, string key);

        object GetInstance(Type service, string key);

        bool HasHandler(Type service, string key);

        IEnumerable<object> GetAllInstances(Type service);

        void BuildUp(object instance);
    }
}