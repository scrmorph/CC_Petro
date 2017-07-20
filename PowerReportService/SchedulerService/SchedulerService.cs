using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using PowerReport;
using PowerReport.Threading;
using PowerReportService.SettingService;

namespace PowerReportService.SchedulerService
{
    public class SchedulerService : ISchedulerService
    {        
        private Timer _timer;
        private Settings _settings;        
        private Func<InterDayReportGenerator> _scheduledWork;

        private static Logger log = LogManager.GetCurrentClassLogger();

        private void OnTimerCallback(object state)
        {
            var generator = _scheduledWork();
            Task.Factory.StartNew(() => generator.CreateReportAsync(_settings.FailAttemps, _settings.OutputDirectoryPath), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);
        }

        public void SetupScheduler(Settings settingses, Func<InterDayReportGenerator> scheduledWork)
        {
            try
            {
                if (_timer == null)
                {
                    _settings = settingses;
                    _scheduledWork = scheduledWork;
                    _timer = new Timer(OnTimerCallback, new object(), TimeSpan.Zero, settingses.ReportTimeInterval);

                    if (log.IsInfoEnabled)
                        log.Info($"Timer has been created with time interval: {settingses.ReportTimeInterval}");
                }
                else
                {
                    throw new ApplicationException("Scheduler has been already created.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public void StopScheduler()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
