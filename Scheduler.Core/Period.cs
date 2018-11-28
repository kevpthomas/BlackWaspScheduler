using System;

namespace Scheduler.Core
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
 
            if (Start > End)
            {
                throw new ArgumentException("The start date may not be after the end date.");
            }
        }
    }}