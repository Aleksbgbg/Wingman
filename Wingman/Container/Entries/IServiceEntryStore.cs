namespace Wingman.Container.Entries
{
    using Wingman.Container.Strategies;

    internal interface IServiceEntryStore
    {
        bool HasHandler(ServiceEntry serviceEntry);

        void InsertHandler(ServiceEntry serviceEntry, IServiceLocationStrategy serviceLocationStrategy);

        void RemoveHandler(ServiceEntry serviceEntry);
    }
}