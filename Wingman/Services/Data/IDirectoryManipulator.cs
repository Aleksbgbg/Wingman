namespace Wingman.Services.Data
{
    internal interface IDirectoryManipulator
    {
        string AppDataPath { get; }

        void CreateDirectory(string path);
    }
}