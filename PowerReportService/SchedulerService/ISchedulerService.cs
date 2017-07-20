using System;
using PowerReport;
using PowerReportService.SettingService;

namespace PowerReportService.SchedulerService
{
    public interface ISchedulerService
    {
        void SetupScheduler(Settings settingses, Func<InterDayReportGenerator> interDayReportGenerator);
        void StopScheduler();
    }
}
