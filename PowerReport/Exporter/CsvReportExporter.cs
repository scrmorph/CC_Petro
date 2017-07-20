using System;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NLog;

namespace PowerReport.Exporter
{
    /// <summary>
    /// Exports report to Csv File
    /// </summary>
    public class CsvReportExporter : IReportExporter
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private Func<string, TextWriter> _streamFactory;

        public CsvReportExporter():this(null)
        {
        }

        public CsvReportExporter(Func<string, TextWriter> factory = null)
        {
            _streamFactory = factory ?? new Func<string, TextWriter>((path) =>
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return new StreamWriter(path);
            });
        }

            
        public void Export(string destination, DateTime extractDateTime, InterdayPowerReportPosition[] positions)
        {
            try
            {
                var path = CreateFullOutputPath(destination, extractDateTime);

                if (log.IsInfoEnabled)
                    log.Info($"Create Report file {path}");      

                using (var streamWriter = _streamFactory(path))
                {
                    var writer = new CsvWriter(streamWriter, CreateCvConfiguration());
                    
                    writer.WriteHeader<InterdayPowerReportPosition>();
                    foreach (var position in positions)
                    {
                        writer.WriteRecord(position);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Report Export Exception: ", ex);
                throw;
            }       
        }

        /// <summary>
        /// Creates configuration for CsvWriter.         
        /// </summary>
        private CsvConfiguration CreateCvConfiguration()
        {
            var cofnig = new CsvConfiguration() { HasHeaderRecord = true, Delimiter = "\t" };
            cofnig.RegisterClassMap<CvsPositionMap>();
            return cofnig;
        }

        /// <summary>
        /// Creates path containing both file name and output path
        /// </summary>        
        private string CreateFullOutputPath(string outputRoot, DateTime reportTime)
        {
            return Path.Combine(outputRoot, CreateReportFileName(reportTime));
        }

        /// <summary>
        /// Returns formatted report filename
        /// </summary>        
        private string CreateReportFileName(DateTime reportTime)
        {
            return $"PowerPosition_{reportTime:yyyyMMdd}_{reportTime:HHmm}.csv";
        }
    }
}
