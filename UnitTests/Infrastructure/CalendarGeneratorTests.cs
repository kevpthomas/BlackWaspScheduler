using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Scheduler.Core;
using Scheduler.Infrastructure;
using Shouldly;
using Xbehave;

namespace UnitTests.Infrastructure
{
    public class CalendarGeneratorTests : UnitTestBase<CalendarGenerator>
    {
        private TimeSpan CreateTimeOfDay()
        {
            return new TimeSpan(Faker.Random.Int(0, 23), Faker.Random.Int(0, 59), 0);
        }

        [Scenario]
        public void NoSchedulesDuringPeriod(Period period, List<ISchedule> schedules, IEnumerable<Appointment> appointments)
        {
            var periodStart = Faker.Date.Soon();
            var periodEnd = periodStart.AddDays(2);

            var scheduleItem = CreateMock<ISchedule>();
            schedules = new List<ISchedule> { scheduleItem };

            "Given a valid scheduling period"
                .x(() => period = new Period(periodStart, periodEnd));

            "And a schedule outside of the scheduling period"
                .x(() => scheduleItem
                    .Setup(x => x.OccursOnDate(It.IsInRange(periodStart.Date, periodEnd.Date, Range.Inclusive)))
                    .Returns(false));

            "When generating a schedule of appointments"
                .x(() => appointments = TestInstance.GenerateCalendar(period, schedules));

            "Then the schedule contains no appointments"
                .x(() => appointments.ShouldBeEmpty());
        }

        [Scenario]
        public void SchedulesDuringPeriod(Period period, List<ISchedule> schedules, List<Appointment> appointments)
        {
            var periodStart = Faker.Date.Soon();
            var periodEnd = periodStart.AddDays(2);

            var scheduleAtStartOfPeriod = CreateMock<ISchedule>();
            var timeOfDayAtStartOfPeriod = CreateTimeOfDay();
            scheduleAtStartOfPeriod.SetupGet(x => x.TimeOfDay)
                .Returns(timeOfDayAtStartOfPeriod);
            scheduleAtStartOfPeriod.SetupGet(x => x.Name)
                .Returns(nameof(scheduleAtStartOfPeriod));

            var scheduleDuringPeriod = CreateMock<ISchedule>();
            var timeOfDayDuringPeriod = CreateTimeOfDay();
            scheduleDuringPeriod.SetupGet(x => x.TimeOfDay)
                .Returns(timeOfDayDuringPeriod);
            scheduleDuringPeriod.SetupGet(x => x.Name)
                .Returns(nameof(scheduleDuringPeriod));
            
            var scheduleAtEndOfPeriod = CreateMock<ISchedule>();
            var timeOfDayAtEndOfPeriod = CreateTimeOfDay();
            scheduleAtEndOfPeriod.SetupGet(x => x.TimeOfDay)
                .Returns(timeOfDayAtEndOfPeriod);
            scheduleAtEndOfPeriod.SetupGet(x => x.Name)
                .Returns(nameof(scheduleAtEndOfPeriod));

            schedules = new List<ISchedule>
            {
                scheduleAtStartOfPeriod,
                scheduleDuringPeriod,
                scheduleAtEndOfPeriod
            };

            "Given a valid scheduling period"
                .x(() => period = new Period(periodStart, periodEnd));

            "And a schedule at the beginning of the scheduling period"
                .x(() => scheduleAtStartOfPeriod
                    .Setup(x => x.OccursOnDate(periodStart.Date))
                    .Returns(true));

            "And a schedule during the scheduling period"
                .x(() => scheduleDuringPeriod
                    .Setup(x => x.OccursOnDate(periodStart.Date.AddDays(1)))
                    .Returns(true));
            
            "And a schedule at the end of the scheduling period"
                .x(() => scheduleAtEndOfPeriod
                    .Setup(x => x.OccursOnDate(periodEnd.Date))
                    .Returns(true));

            "When generating a schedule of appointments"
                .x(() => appointments = TestInstance.GenerateCalendar(period, schedules).ToList());

            "Then the schedule contains an appointment for each schedule item"
                .x(() => appointments.ShouldSatisfyAllConditions(
                    () => appointments.SingleOrDefault(a => a.Name.Equals(scheduleAtStartOfPeriod.Name)).ShouldNotBeNull(),
                    () => appointments.SingleOrDefault(a => a.Name.Equals(scheduleDuringPeriod.Name)).ShouldNotBeNull(),
                    () => appointments.SingleOrDefault(a => a.Name.Equals(scheduleAtEndOfPeriod.Name)).ShouldNotBeNull()
                ));

            "And each appointment has the corresponding schedule time of day"
                .x(() => appointments.ShouldSatisfyAllConditions(
                    () => appointments.Single(a => a.Name.Equals(scheduleAtStartOfPeriod.Name)).Time.TimeOfDay.ShouldBe(scheduleAtStartOfPeriod.TimeOfDay),
                    () => appointments.Single(a => a.Name.Equals(scheduleDuringPeriod.Name)).Time.TimeOfDay.ShouldBe(scheduleDuringPeriod.TimeOfDay),
                    () => appointments.Single(a => a.Name.Equals(scheduleAtEndOfPeriod.Name)).Time.TimeOfDay.ShouldBe(scheduleAtEndOfPeriod.TimeOfDay)
                ));

            "And the appointments are sorted in chronological order"
                .x(() => appointments.ShouldSatisfyAllConditions(
                    () => appointments[0].Time.ShouldBeLessThanOrEqualTo(appointments[1].Time),
                    () => appointments[1].Time.ShouldBeLessThanOrEqualTo(appointments[2].Time)
                ));
        }
    }
}