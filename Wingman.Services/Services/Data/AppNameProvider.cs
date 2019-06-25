namespace Wingman.Services.Data
{
    internal class AppNameProvider : IAppNameProvider
    {
        internal AppNameProvider(string appName)
        {
            AppName = appName;
        }

        public string AppName { get; }
    }
}