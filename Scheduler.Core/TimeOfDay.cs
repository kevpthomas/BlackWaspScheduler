using System;
using Ardalis.GuardClauses;

namespace Scheduler.Core
{
    /// <summary>
    /// Represents the time of day for a schedule item.
    /// </summary>
    /// <remarks>
    /// Avoid primitive obsession code smell using technique from
    /// https://lostechies.com/jimmybogard/2007/12/03/dealing-with-primitive-obsession/
    /// </remarks>
    public class TimeOfDay
    {
        public TimeOfDay(uint hours, uint minutes)
        {
            Guard.Against.OutOfRange(hours, nameof(hours), 0, 23);
            Guard.Against.OutOfRange(minutes, nameof(minutes), 0, 59);

            Value = new TimeSpan((int)hours, (int)minutes, 0);
        }

        public TimeSpan Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator TimeSpan(TimeOfDay timeOfDate)
        {
            return timeOfDate.Value;
        }

        public static explicit operator TimeOfDay(TimeSpan value)
        {
            return new TimeOfDay((uint)value.Hours, (uint)value.Minutes);
        }
    }
}