using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PowerReport;
using PowerReport.Exporter;
using PowerReport.Threading;
using PowerReportTest.QualityTools.DateProvider;
using PowerReportTest.QualityTools.Threading;
using Services;

namespace PowerReportTest.SchedulerService
{
    [TestFixture]
    public class Given_Genereting_Report
    {
        [SetUp]
        public void SetUp()
        {
            BackgroundScheduler.Default = new InstantBackgroundScheduler();
        }

        [Test]
        public void When_Report_Is_Scheduled_Then_Power_Service_Is_Called()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 06), 24);       
            powerService.GetTradesAsync(new DateTime(2016, 11, 07)).Returns(new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            powerService.Received(1).GetTradesAsync(Arg.Any<DateTime>());
        }

        [Test]
        public void When_Report_Is_Scheduled_Before_23_Then_Power_Service_Is_Called_With_The_Same_Date()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 06, 14, 34, 23));
            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 06), 24);
            powerService.GetTradesAsync(new DateTime(2016, 11, 06)).Returns(new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            powerService.Received(1).GetTradesAsync(Arg.Is<DateTime>(o => o == new DateTime(2016, 11, 06)));
        }

        [Test]
        public void When_Report_Is_Scheduled_After_23_But_Before_00_Then_Power_Service_Is_Called_With_The_Next_Day_Date()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 06, 23, 00, 00));

            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 07), 24);
            powerService.GetTradesAsync(new DateTime(2016, 11, 07)).Returns(new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(1, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            powerService.Received(1).GetTradesAsync(Arg.Is<DateTime>(o => o == new DateTime(2016, 11, 07)));
        }

        [Test]
        public void When_Report_Is_Crashed_Then_Retry()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 06, 23, 00, 00));
            
            powerService.GetTradesAsync(Arg.Any<DateTime>()).Throws(new Exception());

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(3, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            powerService.Received(3).GetTradesAsync(Arg.Any<DateTime>());
        }

        [Test]
        public void When_Report_Is_Crashed_And_Recover_Then_Report_Generated()
        {
            //Arrange
            var powerService = Substitute.For<IPowerService>();
            var exporter = Substitute.For<IReportExporter>();
            var dataProvider = new FakeReportDateProvider();
            var sut = new InterDayReportGenerator(powerService, exporter, dataProvider);

            dataProvider.WithCurrentDateAndTime(new DateTime(2016, 11, 06, 23, 00, 00));
            var powerTrade = PowerTrade.Create(new DateTime(2016, 11, 07), 24);
      
            powerService.GetTradesAsync(Arg.Any<DateTime>()).Returns(x => throw new ArgumentException(), x => new[] { powerTrade });

            //Act
            Task.Factory.StartNew(async () => await sut.CreateReportAsync(3, ""), CancellationToken.None, TaskCreationOptions.None, BackgroundScheduler.Default);

            //Assert
            powerService.Received(2).GetTradesAsync(Arg.Any<DateTime>());
        }
    }
}
