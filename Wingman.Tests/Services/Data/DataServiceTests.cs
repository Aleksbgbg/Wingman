namespace Wingman.Tests.Services.Data
{
    using System;

    using Moq;

    using Wingman.Services;
    using Wingman.Services.Data;

    using Xunit;

    public class DataServiceTests
    {
        private const string DataName = "DataName";

        private readonly Mock<IDataSerializer> _dataSerializerMock;

        private readonly Mock<IPersistentStore> _persistentStoreMock;

        private readonly DataService _dataService;

        private bool _containsData = true;

        public DataServiceTests()
        {
            _dataSerializerMock = new Mock<IDataSerializer>();

            _persistentStoreMock = new Mock<IPersistentStore>();
            _persistentStoreMock.Setup(store => store.Contains(DataName))
                                .Returns(() => _containsData);

            _dataService = new DataService(_dataSerializerMock.Object, _persistentStoreMock.Object);
        }

        [Fact]
        public void TestSave()
        {
            object data = new object();
            string serialized = SetupSerialize(data);

            _dataService.Save(DataName, data);

            VerifySaveToPersistentStore(DataName, serialized);
        }

        [Fact]
        public void TestLoad()
        {
            string dataString = SetupLoad(DataName);
            Data expectedData = SetupDeserialize(dataString);

            Data actualData = _dataService.Load<Data>(DataName);

            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void TestLoadNonExistentDataReturnsDefaultObject()
        {
            SetupDoesNotHaveSavedData();
            Data expectedData = new Data();

            Data actualData = _dataService.Load(DataName, expectedData);

            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void TestLoadFunc()
        {
            string dataString = SetupLoad(DataName);
            Data expectedData = SetupDeserialize(dataString);

            Data actualData = _dataService.Load<Data>(DataName, () => null);

            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void TestLoadFuncNonExistentDataReturnsDefaultObject()
        {
            SetupDoesNotHaveSavedData();
            Data expectedData = new Data();

            Data actualData = _dataService.Load(DataName, () => expectedData);

            Assert.Equal(expectedData, actualData);
        }

        private string SetupSerialize(object data)
        {
            string toString = Guid.NewGuid().ToString();

            _dataSerializerMock.Setup(serializer => serializer.Serialize(data))
                               .Returns(toString);

            return toString;
        }

        private string SetupLoad(string dataName)
        {
            string dataString = Guid.NewGuid().ToString();

            _persistentStoreMock.Setup(store => store.Load(dataName))
                                .Returns(dataString);

            return dataString;
        }

        private Data SetupDeserialize(string dataString)
        {
            Data data = new Data();

            _dataSerializerMock.Setup(serializer => serializer.Deserialize<Data>(dataString))
                               .Returns(data);

            return data;
        }

        private void SetupDoesNotHaveSavedData()
        {
            _containsData = false;
        }

        private void VerifySaveToPersistentStore(string dataName, string value)
        {
            _persistentStoreMock.Verify(store => store.Save(dataName, value));
        }

        private class Data { }
    }
}