using System;
using System.Collections.Generic;
using Scheduler.Core;
using Scheduler.Core.Entities;
using Scheduler.Core.Interfaces;
using Scheduler.Core.Types;
using TinyIoC;

namespace Scheduler.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleIoCConfig.Instance.Register();
 
            Console.Clear();

            var single1 = new SingleSchedule
            {
                Name = "Meet Bob for Pint",
                TimeOfDay = new TimeOfDay(19, 30),
                Date = new DateTime(2012, 5, 8)
            };
 
            var single2 = new SingleSchedule
            {
                Name = "Confirm Meeting",
                TimeOfDay = new TimeOfDay(9, 30),
                Date = new DateTime(2012, 5, 12)
            };
 
            var simple = new SimpleRepeatingSchedule
            {
                Name = "Sprint Planning Meeting",
                TimeOfDay = new TimeOfDay(10, 0),
                SchedulingRange = new Period(new DateTime(2012, 1, 2), new DateTime(2012, 12, 31)),
                DaysBetween = 7
            };

            var weekly = new WeeklySchedule
            {
                Name = "Check Backup Reliability",
                TimeOfDay = new TimeOfDay(8, 0),
                SchedulingRange = new Period(new DateTime(2012, 5, 28), new DateTime(2012, 6, 8))
            };
            weekly.SetDays(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday });
 
            var monthly = new MonthlySchedule
            {
                Name = "Check Wages",
                TimeOfDay = new TimeOfDay(18, 0),
                DayOfMonth = (DayOfMonth)31,
                SchedulingRange = new Period(new DateTime(2012, 1, 2), new DateTime(2100, 1, 1))
            };

            var schedules = new List<ISchedule> { single1, single2, simple ,weekly, monthly };

            var generator = TinyIoCContainer.Current.Resolve<ICalendarGenerator>();
            var period = new Period(new DateTime(2012, 5, 1), new DateTime(2012, 6, 30));
            var appointments = generator.GenerateCalendar(period, schedules);
 
            foreach (var appointment in appointments)
            {
                Console.WriteLine("{0:yyyy-MM-dd HH:mm} | {1}", appointment.Time, appointment.Name);
            }

            Console.Read();
        }
    }
}
