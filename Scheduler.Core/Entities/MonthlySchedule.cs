using System;
using Scheduler.Core.Types;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// Schedule for a pattern of appointments that occur on the same date of every month.
    /// When the specified day number is beyond a month end, the final date of the month is scheduled instead.
    /// </summary>
    public class MonthlySchedule : RepeatingSchedule
    {
        //TODO: unit tests for MonthlySchedule

        public DayOfMonth DayOfMonth { get; set; }
 
        public override bool OccursOnDate(DateTime date)
        {
            return DateIsInPeriod(date) & IsOnCorrectDate(date);
        }
 
        private bool IsOnCorrectDate(DateTime date)
        {
            if (date.Day == DayOfMonth) return true;

            return date.Day == DateTime.DaysInMonth(date.Year, date.Month)
                   && DayOfMonth > date.Day;
        }
    }}