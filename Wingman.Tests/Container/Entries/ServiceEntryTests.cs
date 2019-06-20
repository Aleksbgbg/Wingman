namespace Wingman.Tests.Container.Entries
{
    using Wingman.Container.Entries;

    using Xunit;

    public class ServiceEntryTests
    {
        [Fact]
        public void TestIdenticalEntriesIdenticalHashCode()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), "SomeKey");
            ServiceEntry serviceEntry1 = new ServiceEntry(typeof(IService), "SomeKey");

            int hashCode0 = serviceEntry0.GetHashCode();
            int hashCode1 = serviceEntry1.GetHashCode();

            Assert.Equal(hashCode0, hashCode1);
        }

        [Fact]
        public void TestNullServiceMatches()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), null);
            ServiceEntry serviceEntry1 = new ServiceEntry(typeof(IService), null);

            int hashCode0 = serviceEntry0.GetHashCode();
            int hashCode1 = serviceEntry1.GetHashCode();

            Assert.Equal(hashCode0, hashCode1);
        }

        [Fact]
        public void TestNullKeyMatches()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(null, "SomeKey");
            ServiceEntry serviceEntry1 = new ServiceEntry(null, "SomeKey");

            int hashCode0 = serviceEntry0.GetHashCode();
            int hashCode1 = serviceEntry1.GetHashCode();

            Assert.Equal(hashCode0, hashCode1);
        }

        [Fact]
        public void TestDifferentEntriesDifferentHashCode()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), "SomeKey");
            ServiceEntry serviceEntry1 = new ServiceEntry(typeof(IService1), "SomeKey");

            int hashCode0 = serviceEntry0.GetHashCode();
            int hashCode1 = serviceEntry1.GetHashCode();

            Assert.NotEqual(hashCode0, hashCode1);
        }

        [Fact]
        public void TestEqualsNull()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), "SomeKey");
            ServiceEntry serviceEntry1 = null;

            bool equals = serviceEntry0.Equals(serviceEntry1);

            Assert.False(equals);
        }

        [Fact]
        public void TestEqualsTrue()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), "SomeKey");
            ServiceEntry serviceEntry1 = new ServiceEntry(typeof(IService), "SomeKey");

            bool equals = serviceEntry0.Equals(serviceEntry1);

            Assert.True(equals);
        }

        [Fact]
        public void TestEqualsFalse()
        {
            ServiceEntry serviceEntry0 = new ServiceEntry(typeof(IService), "SomeKey");
            ServiceEntry serviceEntry1 = new ServiceEntry(typeof(IService1), "SomeKey");

            bool equals = serviceEntry0.Equals(serviceEntry1);

            Assert.False(equals);
        }

        private interface IService { }

        private interface IService1 { }
    }
}