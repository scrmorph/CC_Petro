using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerReport.Exporter
{
    public struct ForwardReportPosition
    {
        private readonly int _positionId;
        private readonly TimeSpan _hour;
        private readonly double _volume;

        public int PositionId => _positionId;

        public TimeSpan Hour => _hour;

        public double Volume => _volume;

        public ForwardReportPosition(int positionId, TimeSpan hour, double volume)
        {
            _positionId = positionId;
            _hour = hour;
            _volume = volume;
        }
    }
}
