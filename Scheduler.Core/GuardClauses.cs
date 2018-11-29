using System;

// ReSharper disable once CheckNamespace
namespace Ardalis.GuardClauses
{
    public static class FooGuard
    {
        public static void StartAfterEnd(this IGuardClause guardClause, DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException("The start date may not be after the end date.");
            }
        }

        public static void OutOfRange(this IGuardClause guardClause, uint input, string parameterName, uint rangeFrom, uint rangeTo)
        {
            guardClause.OutOfRange((int)input, parameterName, (int)rangeFrom, (int)rangeTo);
        }
    }
}
