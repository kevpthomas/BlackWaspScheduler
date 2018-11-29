using System;
using Ardalis.GuardClauses;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// Represents a period of time, starting on one date and finishing on another. Periods do not include time elements.
    /// </summary>
    public class Period
    {
        public DateTime Start { get; }
         
        public DateTime End { get; }
 
        public Period(DateTime start, DateTime end)
        {
            Start = start.Date;
            End = end.Date;

            Guard.Against.StartAfterEnd(Start, End);
        }
    }
}