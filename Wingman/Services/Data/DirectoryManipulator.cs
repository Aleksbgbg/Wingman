namespace Wingman.Services.Data
{
    using System;
    using System.IO;

    internal class DirectoryManipulator : IDirectoryManipulator
    {
        public string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}