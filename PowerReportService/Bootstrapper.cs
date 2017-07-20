using PowerReport;
using PowerReport.DateProvider;
using PowerReport.Exporter;
using PowerReportService.SchedulerService;
using PowerReportService.SettingService;
using Services;
using TinyIoC;

namespace PowerReportService
{
    /// <summary>
    /// Configures IoC container. There is no need to use IoC container 
    /// in this project. It was added to show that it could be used to 
    /// instantiate required types.
    /// </summary>
    public static class Bootstrapper
    {
        public static void Register()
        {
            TinyIoCContainer.Current.Register<IReportDateProvider,InterDayReportReportDateProvider>().AsSingleton();
            TinyIoCContainer.Current.Register<IPowerService,PowerService>().AsSingleton();            
            TinyIoCContainer.Current.Register<IReportExporter, CsvReportExporter>().UsingConstructor(() => new CsvReportExporter());            
            TinyIoCContainer.Current.Register<ISettingService, AppSettingService>().AsSingleton();
            TinyIoCContainer.Current.Register<ISchedulerService, SchedulerService.SchedulerService>().AsSingleton();
            TinyIoCContainer.Current.Register<InterDayReportGenerator>().AsMultiInstance();
        }        
    }
}
