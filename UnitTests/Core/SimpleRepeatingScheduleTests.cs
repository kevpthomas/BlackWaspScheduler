using System;
using Scheduler.Core.Entities;
using Shouldly;
using Xbehave;

namespace UnitTests.Core
{
    public class SimpleRepeatingScheduleTests : UnitTestBase
    {
        [Scenario]
        public void DateBeforeScheduleRange(DateTime date, bool isOnDate, SimpleRepeatingSchedule testInstance)
        {
            "Given a date before the repeating schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SimpleRepeatingSchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(1, 10)), date.AddDays(Faker.Random.Int(11, 20)))};
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateAfterScheduleRange(DateTime date, bool isOnDate, SimpleRepeatingSchedule testInstance)
        {
            "Given a date after the repeating schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SimpleRepeatingSchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-20, -11)), date.AddDays(Faker.Random.Int(-10, -1)))};
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnWrongDay(DateTime date, uint daysBetween, bool isOnDate, SimpleRepeatingSchedule testInstance)
        {
            daysBetween = Faker.Random.UInt(3, 9);

            "Given a date during the repeating schedule period but not on the repeating schedule"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SimpleRepeatingSchedule
                    {
                        DaysBetween = daysBetween,
                        SchedulingRange = new Period(date.AddDays(-daysBetween + 1), date.AddDays(Faker.Random.UInt(daysBetween, daysBetween + 10)))
                    };
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnScheduledDay(DateTime date, uint daysBetween, bool isOnDate, SimpleRepeatingSchedule testInstance)
        {
            daysBetween = Faker.Random.UInt(3, 9);

            "Given a date during the repeating schedule period and not on the repeating schedule"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SimpleRepeatingSchedule
                    {
                        DaysBetween = daysBetween,
                        SchedulingRange = new Period(date.AddDays(-daysBetween), date.AddDays(Faker.Random.UInt(daysBetween, daysBetween + 10)))
                    };
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule contains the date"
                .x(() => isOnDate.ShouldBe(true));
        }
    }
}