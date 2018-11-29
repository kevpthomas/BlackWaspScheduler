using System;
using System.Collections.Generic;
using Scheduler.Core;
using Scheduler.Core.Entities;
using Shouldly;
using Xbehave;

namespace UnitTests.Core
{
    public class WeeklyScheduleTests : UnitTestBase
    {
        [Scenario]
        public void DateBeforeScheduleRange(DateTime date, bool isOnDate, WeeklySchedule testInstance)
        {
            "Given a date before the weekly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new WeeklySchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(1, 10)), date.AddDays(Faker.Random.Int(11, 20)))};
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateAfterScheduleRange(DateTime date, bool isOnDate, WeeklySchedule testInstance)
        {
            "Given a date after the weekly schedule period"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new WeeklySchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-20, -11)), date.AddDays(Faker.Random.Int(-10, -1)))};
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnWrongDay(DateTime date, List<DayOfWeek> days, bool isOnDate, WeeklySchedule testInstance)
        {
            days = new List<DayOfWeek>();

            "Given a date on the schedule date"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new WeeklySchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-10, 0)), date.AddDays(Faker.Random.Int(1, 10)))};
                });
            
            "And the date is on a different day of week from the weekly schedule period"
                .x(() =>
                {
                    days.Add(Faker.Random.Enum(date.DayOfWeek));
                    testInstance.SetDays(days);
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not contain the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateDuringScheduleRangeOnScheduledDay(DateTime date, List<DayOfWeek> days, bool isOnDate, WeeklySchedule testInstance)
        {
            days = new List<DayOfWeek>();

            "Given a date on the schedule date"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new WeeklySchedule {SchedulingRange = new Period(date.AddDays(Faker.Random.Int(-10, 0)), date.AddDays(Faker.Random.Int(1, 10)))};
                });
            
            "And the date is on a day of week matching the weekly schedule period"
                .x(() =>
                {
                    days.Add(date.DayOfWeek);
                    testInstance.SetDays(days);
                });

            "When I check if the schedule is during the schedule period"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule contains the date"
                .x(() => isOnDate.ShouldBe(true));
        }
    }
}