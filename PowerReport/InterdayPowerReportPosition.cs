using System;

namespace PowerReport
{
    /// <summary>
    /// InterDay Report Position
    /// </summary>
    public struct InterdayPowerReportPosition
    {
        /// <summary>
        /// Start Hour
        /// </summary>
        public TimeSpan Hour { get; }

        /// <summary>
        /// Aggreggated volume
        /// </summary>
        public double Volume { get; }

        /// <summary>
        /// Position Id, the same as PowerTrade->Position->PositionId
        /// </summary>
        public int  PositionId { get; }

        public InterdayPowerReportPosition(int positionId, double volume)
        {
            PositionId = positionId;
            Hour = positionId == 1 ? TimeSpan.FromHours(23) : TimeSpan.FromHours(positionId - 2);
            Volume = volume;
        }

    }
}
