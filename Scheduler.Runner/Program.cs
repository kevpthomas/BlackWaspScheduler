using System;
using System.Collections.Generic;
using Scheduler.Core;
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
                TimeOfDay = new TimeSpan(19, 30, 0),
                Date = new DateTime(2012, 5, 8)
            };
 
            var single2 = new SingleSchedule
            {
                Name = "Confirm Meeting",
                TimeOfDay = new TimeSpan(9, 30, 0),
                Date = new DateTime(2012, 5, 12)
            };
 
            var schedules = new List<ISchedule> { single1, single2 };

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
