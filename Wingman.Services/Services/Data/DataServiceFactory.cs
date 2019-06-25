namespace Wingman.Services.Data
{
    /// <summary> Factory that creates a default implementation of <see cref="IDataService"/> which saves to %AppData%/YourAppName using JSON formatting. </summary>
    public static class DataServiceFactory
    {
        public static IDataService Create(string appName)
        {
            return new DataService(new JsonSerializer(),
                                   new AppDataStore(new AppNameProvider(appName),
                                                    new DirectoryManipulator(),
                                                    new FileManipulator()
                                   )
            );
        }
    }
}