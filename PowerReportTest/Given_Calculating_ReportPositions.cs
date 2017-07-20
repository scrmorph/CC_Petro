using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PowerReport;
using PowerReport.Exporter;
using PowerReport.Threading;
using PowerReportTest.QualityTools.DateProvider;
using PowerReportTest.QualityTools.Threading;
using Services;

namespace PowerReportTest.Report
{
    [TestFixture]
    public class Given_Calculating_ReportPositions
    {
        [SetUp]
        public void SetUp()
        {
            BackgroundScheduler.Default = new InstantBackgroundScheduler();
        }

        [Test]
        public void When_PowerTrade_Has_Not_The_Same_Date_As_Report_Then_Is_Not_Calculated()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 06), 24);
            for (int i = 0; i < powerTrade.Periods.Length; i++)
            {
                powerTrade.Periods[i].Volume = i;
            }

            powerService.GetTradesAsync(new DateTime(2016, 11, 07)).Returns(new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            exporter.Received().Export(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Is<InterdayPowerReportPosition[]>(o => o.Length == 0));
        }

        [Test]
        public void When_PowerTrade_Has_The_Same_Date_As_Report_Then_Is_Calculated()
        {
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 07), 24);
            for (int i = 0; i < powerTrade.Periods.Length; i++)
            {
                powerTrade.Periods[i].Volume = i;
            }
            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 07));
            powerService.GetTradesAsync(new DateTime(2016, 11, 07)).Returns(new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            exporter.Received().Export(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Is<InterdayPowerReportPosition[]>(o => o.Length == 24 && o[6].Volume == 6));

        }

        [Test]
        public void When_Positions_Have_The_Same_Period_Then_Are_Sumed()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            var firstPowerTrade = PowerTrade.Create(new DateTime(2016, 11, 05), 24);
            var secondPowerTrade = PowerTrade.Create(new DateTime(2016, 11, 05), 24);

            var trades = new[] { firstPowerTrade, secondPowerTrade };

            for (int k = 0; k < trades.Length; k++)
            {
                for (int i = 0; i < trades[k].Periods.Length; i++)
                {
                    trades[k].Periods[i].Volume = i;
                }
            }
            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 05));
            powerService.GetTradesAsync(new DateTime(2016, 11, 05)).Returns(trades);

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            exporter.Received().Export(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Is<InterdayPowerReportPosition[]>(o => o.Length == 24 && o[6].Volume == 12));
        }
    }
}
