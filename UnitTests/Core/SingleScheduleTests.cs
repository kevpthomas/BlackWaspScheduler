using System;
using Scheduler.Core;
using Shouldly;
using Xbehave;

namespace UnitTests.Core
{
    public class SingleScheduleTests : UnitTestBase
    {
        [Scenario]
        public void DateBeforeScheduleDate(DateTime date, bool isOnDate, SingleSchedule testInstance)
        {
            "Given a date before the schedule date"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SingleSchedule {Date = date.AddDays(Faker.Random.Int(1, 1000))};
                });

            "When I check if the schedule is on the date"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not occur on the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateAfterScheduleDate(DateTime date, bool isOnDate, SingleSchedule testInstance)
        {
            "Given a date after the schedule date"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SingleSchedule {Date = date.AddDays(-Faker.Random.Int(1, 1000))};
                });

            "When I check if the schedule is on the date"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule does not occur on the date"
                .x(() => isOnDate.ShouldBe(false));
        }

        [Scenario]
        public void DateOnScheduleDate(DateTime date, bool isOnDate, SingleSchedule testInstance)
        {
            "Given a date on the schedule date"
                .x(() =>
                {
                    date = Faker.Date.Soon().Date;
                    testInstance = new SingleSchedule {Date = date};
                });

            "When I check if the schedule is on the date"
                .x(() => isOnDate = testInstance.OccursOnDate(date));

            "Then the schedule occurs on the date"
                .x(() => isOnDate.ShouldBe(true));
        }
    }
}