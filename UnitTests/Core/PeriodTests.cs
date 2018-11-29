using System;
using Scheduler.Core.Entities;
using Shouldly;
using Xbehave;
using Xunit;

namespace UnitTests.Core
{
    public class PeriodTests : UnitTestBase
    {
        [Scenario]
        public void StartDateAfterEndDate(DateTime startDate, DateTime endDate, Exception ex, Period testInstance)
        {
            "Given a start date after an end date"
                .x(() =>
                {
                    startDate = Faker.Date.Soon();
                    endDate = startDate.AddDays(-Faker.Random.Double(1, short.MaxValue));
                });

            "When I define a scheduler period"
                .x(() => ex = Record.Exception(() => testInstance = new Period(startDate, endDate)));

            "Then the period is invalid"
                .x(() => ex.ShouldBeOfType(typeof(ArgumentException)));
        }

        [Scenario]
        public void StartDateSameAsEndDate(DateTime startDate, DateTime endDate, Exception ex, Period testInstance)
        {
            "Given a start date and a matching end date"
                .x(() =>
                {
                    startDate = Faker.Date.Soon();
                    endDate = startDate.Date.AddHours(Faker.Random.Int(0, 23));
                });

            "When I define a scheduler period"
                .x(() => ex = Record.Exception(() => testInstance = new Period(startDate, endDate)));

            "Then the period is valid"
                .x(() => ex.ShouldBeNull());

            "And the period start date matches the period end date"
                .x(() => testInstance.Start.ShouldBe(testInstance.End));
        }

        [Scenario]
        public void StartDateBeforeEndDate(DateTime startDate, DateTime endDate, Exception ex, Period testInstance)
        {
            "Given a start date before an end date"
                .x(() =>
                {
                    startDate = Faker.Date.Soon();
                    endDate = startDate.Date.AddHours(Faker.Random.Int(24, short.MaxValue));
                });

            "When I define a scheduler period"
                .x(() => ex = Record.Exception(() => testInstance = new Period(startDate, endDate)));

            "Then the period is valid"
                .x(() => ex.ShouldBeNull());

            "And the period end date is after the period start date"
                .x(() => testInstance.End.ShouldBeGreaterThan(testInstance.Start));
        }
    }
}