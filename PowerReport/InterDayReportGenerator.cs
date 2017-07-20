using System;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using PowerReport.DateProvider;
using PowerReport.Exporter;
using PowerReport.Extensions;
using PowerReport.Threading;
using Services;

namespace PowerReport
{
    /// <summary>
    /// Main class responsible for generating InterDay report.
    /// </summary>
    public class InterDayReportGenerator
    {
        private readonly IPowerService _powerService;
        private readonly IReportExporter _exporter;
        private readonly IReportDateProvider _dateProvider;

        private static Logger log = LogManager.GetCurrentClassLogger();

        public InterDayReportGenerator(IPowerService powerService, 
                                       IReportExporter exporter,
                                       IReportDateProvider dateProvider)
        {
            _powerService = powerService;
            _exporter = exporter;
            _dateProvider = dateProvider;
        }
        
        public async Task CreateReportAsync(int retryAttemps, string outputPath)
        {
            //Gets report date - date used to call PowerService
            var reportDate = _dateProvider.GetReportDate();

            //Repe
            for (int i = 0; i < retryAttemps; i++)
            {
                try
                {
                    if (log.IsInfoEnabled)
                        log.Info($"Creating report attempt {i+1} of {retryAttemps} for report date {reportDate.Date}.");

                    if (log.IsInfoEnabled)
                        log.Info("Retreving power trades.");

                    //Gets PowerTrade 
                    var powerTrades = await _powerService.GetTradesAsync(reportDate);

                    if (log.IsInfoEnabled)
                        log.Info("Power trades retreived.");

                    if (log.IsInfoEnabled)
                        log.Info("Calculating interday report positions.");

                    //Calculates interDay positions. 
                    var interDayPositions = powerTrades.Where(trade => trade.Date.Date == reportDate.Date)
                                                         .SelectMany(trades => trades.Periods)
                                                         .GroupBy(periods => periods.Period)
                                                         .Select(group => new InterdayPowerReportPosition(group.Key, group.Sum(p => p.Volume)))
                                                         .OrderBy(data => data.PositionId)
                                                         .ToArray();

                    if (log.IsInfoEnabled)
                        log.Info("Interday positions calculated.");

                    if (log.IsInfoEnabled)
                        log.Info("Exporting report.");

                    //Exports report, using injected exporter.
                    _exporter.Export(outputPath, _dateProvider.GetExtractDateTime(), interDayPositions);

                    if (log.IsInfoEnabled)
                        log.Info("Report Exported.");

                    if (log.IsInfoEnabled)
                        log.Info("Report has been created successfully.");

                    return;
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Create Report Error.");
                    //If no more attempts we can ommit delay. 
                    if (i + 1 == retryAttemps)
                    {
                        log.Info("Report couldn't be created.");
                        return;
                    }
                }
                //Delays next attempt about 10 second. In the test project, delay is eliminated. 
                await BackgroundScheduler.Default.Delay(TimeSpan.FromSeconds(10));
            }            

        }
    }
}
