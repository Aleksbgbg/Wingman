namespace Wingman.Tests.Container
{
    using Wingman.Container;

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

        private interface IService { }

        private interface IService1 { }
    }
}