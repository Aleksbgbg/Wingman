namespace Wingman.Container.Entries
{
    using System.Collections.Generic;

    using Wingman.Container.Strategies;

    internal interface IServiceEntryStore
    {
        bool HasHandler(ServiceEntry serviceEntry);

        void InsertHandler(ServiceEntry serviceEntry, IServiceLocationStrategy serviceLocationStrategy);

        void RemoveHandler(ServiceEntry serviceEntry);

        IEnumerable<IServiceLocationStrategy> RetrieveHandlers(ServiceEntry serviceEntry);
    }
}