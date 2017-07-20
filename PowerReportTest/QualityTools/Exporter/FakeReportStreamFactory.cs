using System.IO;
using System.Text;

namespace PowerReportTest.QualityTools.Exporter
{
    public class FakeReportStreamFactory 
    {
        public string FilePath { get; set; }
        private StringBuilder _builder;

        public TextWriter CreateTextWriter(string path)
        {
            FilePath = path;
            _builder = new StringBuilder();
            return new StringWriter(_builder);
        }

        public string GetStreamContent()
        {
            return _builder.ToString();
        }
    }
}
