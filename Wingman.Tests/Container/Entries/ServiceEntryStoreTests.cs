namespace Wingman.Tests.Container.Entries
{
    using Wingman.Container.Entries;

    using Xunit;

    public class ServiceEntryStoreTests
    {
        private const string DefaultServiceKey = "Key";

        private readonly ServiceEntryStore _serviceEntryStore;

        public ServiceEntryStoreTests()
        {
            _serviceEntryStore = new ServiceEntryStore();
        }

        [Fact]
        public void TestServiceIsNotRegisteredByDefault()
        {
            bool hasHandler = _serviceEntryStore.HasHandler(DefaultServiceEntry);

            Assert.False(hasHandler);
        }

        [Fact]
        public void TestHasHandlerAfterRegistering()
        {
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, null);

            Assert.True(_serviceEntryStore.HasHandler(DefaultServiceEntry));
        }

        [Fact]
        public void TestInsertDuplicateHandler()
        {
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, null);
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, null);

            Assert.True(_serviceEntryStore.HasHandler(DefaultServiceEntry));
        }

        [Fact]
        public void TestHasHandlerAfterRemoving()
        {
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, null);
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, null);
            _serviceEntryStore.RemoveHandler(DefaultServiceEntry);

            Assert.False(_serviceEntryStore.HasHandler(DefaultServiceEntry));
        }

        private static ServiceEntry DefaultServiceEntry => new ServiceEntry(typeof(IService), DefaultServiceKey);

        private interface IService { }
    }
}