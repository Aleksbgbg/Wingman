namespace Wingman.Services.Data
{
    internal interface IFileManipulator
    {
        bool Exists(string filepath);

        void WriteAllText(string filepath, string contents);

        string ReadAllText(string filepath);
    }
}