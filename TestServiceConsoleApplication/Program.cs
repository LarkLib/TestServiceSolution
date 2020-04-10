using Topshelf;


namespace TestServiceConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<UploadDataService>(s =>
                {
                    s.ConstructUsing(name => new UploadDataService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                //x.RunAsPrompt();
                x.RunAsLocalSystem();
                x.SetDescription("Elite Upload Data Service");
                x.SetDisplayName("Elite Upload Data Service");
                x.SetServiceName("EliteUploadDataService");
            });
        }
    }
}
