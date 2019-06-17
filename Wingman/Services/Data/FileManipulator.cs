namespace Wingman.Services.Data
{
    using System.IO;

    internal class FileManipulator : IFileManipulator
    {
        public bool Exists(string filepath)
        {
            return File.Exists(filepath);
        }

        public void WriteAllText(string filepath, string contents)
        {
            File.WriteAllText(filepath, contents);
        }

        public string ReadAllText(string filepath)
        {
            return File.ReadAllText(filepath);
        }
    }
}