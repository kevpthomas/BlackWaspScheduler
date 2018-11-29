using Ardalis.GuardClauses;

namespace Scheduler.Core.Types
{
    /// <summary>
    /// Represents the day of month for a schedule item.
    /// </summary>
    /// <remarks>
    /// Avoid primitive obsession code smell using technique from
    /// https://lostechies.com/jimmybogard/2007/12/03/dealing-with-primitive-obsession/
    /// </remarks>
    public class DayOfMonth
    {
        public DayOfMonth(uint day)
        {
            Guard.Against.OutOfRange(day, nameof(day), 1, 31);

            Value = day;
        }

        public uint Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator uint(DayOfMonth dayOfMonth)
        {
            return dayOfMonth.Value;
        }

        public static explicit operator DayOfMonth(uint value)
        {
            return new DayOfMonth(value);
        }

        public static explicit operator DayOfMonth(int value)
        {
            return new DayOfMonth((uint)value);
        }
    }
}