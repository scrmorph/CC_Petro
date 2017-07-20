using System;
using FluentAssertions;
using NUnit.Framework;
using PowerReport;
using PowerReport.Exporter;
using PowerReportTest.QualityTools.Exporter;
using Services;

namespace PowerReportTest.Exporterx
{
    [TestFixture]
    class Given_Exporting_Csv
    {

        [Test]
        public void When_Report_Exported_Then_FileName_Contains_ReportDate()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));            

            //Act
            sut.Export("", new DateTime(2001, 07, 16), new InterdayPowerReportPosition[]{});
            var result = streamFactory.FilePath;

            //Assert
            result.Should().Contain("20010716");
        }

        [Test]
        public void When_Report_Exported_Then_FileName_Contains_ReportRunTime()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));           

            //Act
            sut.Export("", new DateTime(2001, 07, 16, 18, 34, 24), new InterdayPowerReportPosition[] { });            
            var result = streamFactory.FilePath;

            //Assert
            result.Should().Contain("1834");
        }

        [Test]
        public void When_Report_Exported_Then_FileName_Is_Correct()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));

            //Act
            sut.Export("", new DateTime(2001, 07, 16, 18, 34, 24), new InterdayPowerReportPosition[] { });
            var result = streamFactory.FilePath;

            //Assert
            result.Should().Be("PowerPosition_20010716_1834.csv");
        }

        [Test]
        public void When_Report_Exported_Then_Path_Uses_Outputpath()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));

            var expectedPath = "Test_Output_Directory";

            //Act
            sut.Export(expectedPath, new DateTime(2001, 07, 16, 18, 34, 24), new InterdayPowerReportPosition[] { });
            var result = streamFactory.FilePath;            

            //Assert
            result.Should().StartWith(expectedPath);
        }

        [Test]
        public void When_Report_Exported_Then_There_Is_Header()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));

            //Act
            sut.Export("", new DateTime(2001, 07, 16, 18, 34, 24), new InterdayPowerReportPosition[] { });
            var result = streamFactory.GetStreamContent();

            //Assert
            result.Should().StartWith("Local Time\tVolume");
        }

        [Test]
        public void When_Report_Exported_Then_Positions_Are_Saved()
        {
            //Arragne
            var streamFactory = new FakeReportStreamFactory();
            var sut = new CsvReportExporter((path) => streamFactory.CreateTextWriter(path));

            //Act
            sut.Export("", new DateTime(2001, 07, 16, 18, 34, 24), new InterdayPowerReportPosition[] { new InterdayPowerReportPosition(1, 1),
                                                                                                       new InterdayPowerReportPosition(2, 2),
                                                                                                       new InterdayPowerReportPosition(3, 3)
                                                                                                     });
            var result = streamFactory.GetStreamContent();

            //Assert
            result.Should().Be("Local Time\tVolume\r\n23:00\t1\r\n00:00\t2\r\n01:00\t3\r\n");
        }

    }
}
