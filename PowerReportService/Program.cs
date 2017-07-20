using NLog;
using Topshelf;


namespace PowerReportService
{
    class Program
    {        

        static void Main(string[] args)
        {
            Bootstrapper.Register();

            HostFactory.Run(x => 
            {
                x.UseNLog();
                x.Service<PowerReportServiceImpl>(s => 
                {
                    s.ConstructUsing(name => TinyIoC.TinyIoCContainer.Current.Resolve<PowerReportServiceImpl>()); 
                    s.WhenStarted(tc => tc.Start()); 
                    s.WhenStopped(tc => tc.Stop());
                    s.WhenPaused(tc => tc.Pause());
                    s.WhenContinued(tc => tc.Continue());
                    s.WhenShutdown(tc => tc.Shutdown());                                        
                });
                x.RunAsLocalSystem();                
                x.StartAutomatically();

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(0);                    
                });                

                //Available only in Topshelf 4.0.3 (.Net 4.5.2)
                //x.AddCommandLineDefinition( ). OnException(ex =>
                //{
                //    logger.Log(LogLevel.Error, ex);
                //});

                x.SetDescription("Power Report Topshelf Host"); 
                x.SetDisplayName("Power Report Service"); 
                x.SetServiceName("PowerReportService"); 
            });

        }
    }
}
