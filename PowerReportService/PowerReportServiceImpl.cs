using NLog;
using PowerReport;
using PowerReportService.SchedulerService;
using PowerReportService.SettingService;
using TinyIoC;

namespace PowerReportService
{
    public class PowerReportServiceImpl
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISchedulerService _schedulerService;
        private readonly ISettingService _settingsService;

        public PowerReportServiceImpl(ISchedulerService service, ISettingService settingService)
        {            
            _schedulerService = service;
            _settingsService = settingService;
        }

        public void Start()
        {
            if (log.IsInfoEnabled)            
                log.Info("Service started.");                
            
            _schedulerService.SetupScheduler(_settingsService.GetSettings(),() => TinyIoCContainer.Current.Resolve<InterDayReportGenerator>());
        }

        public void Stop()
        {            
            _schedulerService.StopScheduler();
            if (log.IsInfoEnabled)
                log.Info("Service stopped.");
        }

        public void Pause()
        {
            if (log.IsInfoEnabled)
                log.Info("Service paused.");
        }

        public void Shutdown()
        {
            if (log.IsInfoEnabled)
                log.Info("Service shutdown.");
        }

        public void Continue()
        {
            if (log.IsInfoEnabled)
                log.Info("Service continues.");
        }
    }
}
