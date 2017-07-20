using System;

namespace PowerReportService.SettingService
{
    public class Settings
    {
        public TimeSpan ReportTimeInterval { get; }
        public int FailAttemps { get; }
        public string OutputDirectoryPath { get; }

        public Settings(int timeOffsetInMinutes, int failAttemps, string path)
        {
            ReportTimeInterval = TimeSpan.FromMinutes(timeOffsetInMinutes);
            FailAttemps = failAttemps;
            OutputDirectoryPath = path;
        }

        public override string ToString()
        {
            return $"Configuration TimeOffset: {ReportTimeInterval:g} FailAttemps: {FailAttemps} OutpuPath: {OutputDirectoryPath}";
        }

        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
                return false;

            Settings settings = (Settings)obj;
            return (settings.FailAttemps == this.FailAttemps) && 
                   (settings.OutputDirectoryPath == this.OutputDirectoryPath) && 
                   (settings.ReportTimeInterval == this.ReportTimeInterval);
        }
    }
}
