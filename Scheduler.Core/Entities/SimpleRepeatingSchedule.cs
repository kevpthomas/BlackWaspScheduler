using System;
using Ardalis.GuardClauses;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// Schedule for recurring appointments that start on a given date and repeat every few days.
    /// The number of days between repetitions is configurable.
    /// </summary>
    public class SimpleRepeatingSchedule : RepeatingSchedule
    {
        private uint _daysBetween;
 
        public uint DaysBetween
        {
            get => _daysBetween;
            set
            {
                Guard.Against.NegativeOrZero((int)value, nameof(value));
                 
                _daysBetween = value;
            }
        }
 
        public override bool OccursOnDate(DateTime date)
        {
            return DateIsInPeriod(date) && DateIsValidForSchedule(date);
        }
 
        private bool DateIsValidForSchedule(DateTime date)
        {
            var daysBetweenFirstAndCheckDate
                = (int)date.Subtract(SchedulingRange.Start).TotalDays;

            return daysBetweenFirstAndCheckDate % DaysBetween == 0;
        }
    }
}