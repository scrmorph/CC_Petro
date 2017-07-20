using System.Configuration;

namespace PowerReportService.SettingService
{
    public class AppSettingService : ISettingService
    {                

        public Settings GetSettings()
        {
            int retryCount = 0;
            int.TryParse(ConfigurationManager.AppSettings["RetryCount"] ?? "0", out retryCount);

            int timeIntervalInMinutes = 0;
            int.TryParse(ConfigurationManager.AppSettings["ReportTimeInterval"] ?? "0", out timeIntervalInMinutes);

            var outputPath = ConfigurationManager.AppSettings["OutputPath"] ?? "";

            return new Settings(timeIntervalInMinutes, retryCount, outputPath);
        }
    }
}
