namespace Wingman.Tests.Services.Data
{
    using System;
    using System.IO;

    using Moq;

    using Wingman.Services.Data;

    using Xunit;

    public class AppDataStoreTests
    {
        private const string AppDataPath = "AppData";

        private const string AppName = "SomeApp";

        private const string DataName = "SomeDataFile";

        private readonly Mock<IDirectoryManipulator> _directoryManipulatorMock;

        private readonly Mock<IFileManipulator> _fileManipulatorMock;

        private readonly AppDataStore _appDataStore;

        public AppDataStoreTests()
        {
            Mock<IAppNameProvider> appNameProviderMock = new Mock<IAppNameProvider>();
            appNameProviderMock.SetupGet(provider => provider.AppName)
                               .Returns(AppName);

            _directoryManipulatorMock = new Mock<IDirectoryManipulator>();
            _directoryManipulatorMock.Setup(manipulator => manipulator.AppDataPath)
                                     .Returns(AppDataPath);

            _fileManipulatorMock = new Mock<IFileManipulator>();

            _appDataStore = new AppDataStore(appNameProviderMock.Object, _directoryManipulatorMock.Object, _fileManipulatorMock.Object);
        }

        [Fact]
        public void TestCreatesAppDataDirectoryInConstructor()
        {
            VerifyCreateAppDataDirectoryCalled();
        }

        [Fact]
        public void TestContainsTrue()
        {
            SetupFileExists(DataName);

            bool contains = _appDataStore.Contains(DataName);

            Assert.True(contains);
            VerifyFileExistsCalled(DataName);
        }

        [Fact]
        public void TestContainsFalse()
        {
            SetupFileDoesntExist(DataName);

            bool contains = _appDataStore.Contains(DataName);

            Assert.False(contains);
            VerifyFileExistsCalled(DataName);
        }

        [Fact]
        public void TestSave()
        {
            string data = Guid.NewGuid().ToString();

            _appDataStore.Save(DataName, data);

            VerifyWriteAllTextCalled(DataName, data);
        }

        [Fact]
        public void TestLoad()
        {
            string actualData = Guid.NewGuid().ToString();
            SetupReadFile(DataName, actualData);

            string loadedData = _appDataStore.Load(DataName);

            Assert.Equal(actualData, loadedData);
        }

        private void SetupFileDoesntExist(string dataName)
        {
            SetupFile(dataName, false);
        }

        private void SetupFileExists(string dataName)
        {
            SetupFile(dataName, true);
        }

        private void SetupFile(string dataName, bool exists)
        {
            _fileManipulatorMock.Setup(manipulator => manipulator.Exists(ToPath(dataName)))
                                .Returns(exists);
        }

        private void SetupReadFile(string dataName, string data)
        {
            _fileManipulatorMock.Setup(manipulator => manipulator.ReadAllText(ToPath(dataName)))
                                .Returns(data);
        }

        private void VerifyFileExistsCalled(string dataName)
        {
            _fileManipulatorMock.Verify(manipulator => manipulator.Exists(ToPath(dataName)));
        }

        private void VerifyWriteAllTextCalled(string dataName, string contents)
        {
            _fileManipulatorMock.Verify(manipulator => manipulator.WriteAllText(ToPath(dataName), contents));
        }

        private void VerifyCreateAppDataDirectoryCalled()
        {
            _directoryManipulatorMock.Verify(manipulator => manipulator.CreateDirectory(Path.Combine(AppDataPath, AppName)));
        }

        private static string ToPath(string dataName)
        {
            return Path.Combine(AppDataPath, AppName, dataName);
        }
    }
}