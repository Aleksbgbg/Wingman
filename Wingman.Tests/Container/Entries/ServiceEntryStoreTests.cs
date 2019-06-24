namespace Wingman.Tests.Container.Entries
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using Wingman.Container.Entries;
    using Wingman.Container.Strategies;

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
            Assert.False(HasHandler());
        }

        [Fact]
        public void TestHasHandlerAfterRegistering()
        {
            InsertNullHandler();

            Assert.True(HasHandler());
        }

        [Fact]
        public void TestInsertDuplicateHandler()
        {
            InsertNullHandler();
            InsertNullHandler();

            Assert.True(HasHandler());
        }

        [Fact]
        public void TestHasHandlerAfterRemoving()
        {
            InsertNullHandler();
            InsertNullHandler();
            RemoveHandler();

            Assert.False(HasHandler());
        }

        [Fact]
        public void TestRetrieveHandlers()
        {
            IServiceLocationStrategy[] serviceLocationStrategies = SetupServiceLocationStrategies(5);

            foreach (IServiceLocationStrategy strategy in serviceLocationStrategies)
            {
                InsertHandler(strategy);
            }

            Assert.Equal(serviceLocationStrategies, RetrieveHandlers());
        }

        [Fact]
        public void TestThrowsWhenNoHandlersRegistered()
        {
            Action retrieve = () => _serviceEntryStore.RetrieveHandlers(DefaultServiceEntry);

            Assert.Throws<KeyNotFoundException>(retrieve);
        }

        private bool HasHandler()
        {
            return _serviceEntryStore.HasHandler(DefaultServiceEntry);
        }

        private void InsertNullHandler()
        {
            InsertHandler(null);
        }

        private void InsertHandler(IServiceLocationStrategy serviceLocationStrategy)
        {
            _serviceEntryStore.InsertHandler(DefaultServiceEntry, serviceLocationStrategy);
        }

        private void RemoveHandler()
        {
            _serviceEntryStore.RemoveHandler(DefaultServiceEntry);
        }

        private IEnumerable<IServiceLocationStrategy> RetrieveHandlers()
        {
            return _serviceEntryStore.RetrieveHandlers(DefaultServiceEntry);
        }

        private static IServiceLocationStrategy[] SetupServiceLocationStrategies(int count)
        {
            IServiceLocationStrategy[] strategies = new IServiceLocationStrategy[count];

            for (int index = 0; index < count; ++index)
            {
                strategies[index] = new Mock<IServiceLocationStrategy>().Object;
            }

            return strategies;
        }

        private static ServiceEntry DefaultServiceEntry => new ServiceEntry(typeof(IService), DefaultServiceKey);

        private interface IService { }
    }
}