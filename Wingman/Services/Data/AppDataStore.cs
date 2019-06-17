namespace Wingman.Services.Data
{
    using System.IO;

    /// <summary> Default implementation of <see cref="IPersistentStore"/>. Stores to AppData under a folder with the app's name. </summary>
    public class AppDataStore : IPersistentStore
    {
        private readonly string _appPath;

        private readonly IFileManipulator _fileManipulator;

        internal AppDataStore(IAppNameProvider appNameProvider, IDirectoryManipulator directoryManipulator, IFileManipulator fileManipulator)
        {
            _appPath = Path.Combine(directoryManipulator.AppDataPath, appNameProvider.AppName);
            _fileManipulator = fileManipulator;

            directoryManipulator.CreateDirectory(_appPath);
        }

        public bool Contains(string dataName)
        {
            return _fileManipulator.Exists(ResolveAppDataPath(dataName));
        }

        public void Save(string dataName, string data)
        {
            _fileManipulator.WriteAllText(ResolveAppDataPath(dataName), data);
        }

        public string Load(string dataName)
        {
            return _fileManipulator.ReadAllText(ResolveAppDataPath(dataName));
        }

        private string ResolveAppDataPath(string dataName)
        {
            return Path.Combine(_appPath, dataName);
        }
    }
}