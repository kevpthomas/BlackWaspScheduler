using System;
using Scheduler.Core.Entities;
using Scheduler.Core.Types;
using Shouldly;
using Xbehave;

namespace UnitTests.Core
{
    public class MonthlyScheduleTests : UnitTestBase
    {
        [Scenario]
        public void DateBeforeScheduleRange(DateTime date, bool isOnDate, MonthlySchedule testInstance)
        {
            "Given a date before the monthly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new MonthlySchedule
                    {
                        SchedulingRange = new Period(date.AddDays(Faker.Random.Int(1, 10)), date.AddDays(Faker.Random.Int(11, 20))),
                        DayOfMonth = (DayOfMonth)Faker.Random.UInt(1, 31)
                    };
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateAfterScheduleRange(DateTime date, bool isOnDate, MonthlySchedule testInstance)
        {
            "Given a date after the monthly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new MonthlySchedule
                    {
                        SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-20, -11)), date.AddDays(Faker.Random.Int(-10, -1))),
                        DayOfMonth = (DayOfMonth)Faker.Random.UInt(1, 31)
                    };
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnWrongDay(DateTime date, bool isOnDate, MonthlySchedule testInstance)
        {
            "Given a date during the monthly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new MonthlySchedule
                    {
                        SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-10, 0)), date.AddDays(Faker.Random.Int(1, 10))),
                    };
                });
            
            "And the date is on a different day of month from the monthly schedule period"
                .x(() =>
                {
                    testInstance.DayOfMonth = (DayOfMonth)Faker.Random.UInt(1, 31);
                    while (testInstance.DayOfMonth == date.Day)
                    {
                        testInstance.DayOfMonth = (DayOfMonth)Faker.Random.UInt(1, 31);
                    }
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnScheduledDay(DateTime date, bool isOnDate, MonthlySchedule testInstance)
        {
            "Given a date during the monthly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new MonthlySchedule
                    {
                        SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-10, 0)), date.AddDays(Faker.Random.Int(1, 10))),
                    };
                });
            
            "And the date is on the same day of month as the monthly schedule period"
                .x(() => testInstance.DayOfMonth = (DayOfMonth)date.Day);

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule contains the date"
                .x(() => isOnDate.ShouldBe(true));
        }

        [Scenario]
        public void DateDuringScheduleRangeBeyondMonthEnd(DateTime date, bool isOnDate, MonthlySchedule testInstance)
        {
            "Given a date on the last day of the month during the monthly schedule period"
                .x(() =>
                {
                    date = new DateTime(Faker.Random.Int(2000, 2100), 4, 30);
                    testInstance = new MonthlySchedule
                    {
                        SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-10, 0)), date.AddDays(Faker.Random.Int(1, 10))),
                    };
                });
            
            "And the schedule day of month is beyond the date's month end"
                .x(() => testInstance.DayOfMonth = (DayOfMonth)31);

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule contains the date by using the final date of the month"
                .x(() => isOnDate.ShouldBe(true));
        }
    }
}