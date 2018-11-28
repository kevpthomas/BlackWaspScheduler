using System;
using System.Collections.Generic;
using System.Linq;
using Scheduler.Core;

namespace Scheduler.Infrastructure
{
    /// <summary>
    /// Generate lists of appointments using rules defined in one or more schedules.
    /// </summary>
    public class CalendarGenerator : ICalendarGenerator
    {
        public IEnumerable<Appointment> GenerateCalendar(
            Period period, IEnumerable<Schedule> schedules)
        {
            var scheduleList = schedules.ToList();

            var appointments = new List<Appointment>();

            for (var checkDate = period.Start; checkDate <= period.End; checkDate = checkDate.AddDays(1))
            {
                AddAppointmentsForDate(checkDate, scheduleList, appointments);
            }

            return appointments.OrderBy(a => a.Time);
        }

        private static void AddAppointmentsForDate(
            DateTime checkDate, IEnumerable<Schedule> schedules, ICollection<Appointment> appointments)
        {
            foreach (var schedule in schedules)
            {
                if (schedule.OccursOnDate(checkDate))
                {
                    appointments.Add(GenerateAppointment(checkDate, schedule));
                }
            }
        }

        private static Appointment GenerateAppointment(DateTime checkDate, Schedule schedule)
        {
            return new Appointment
            {
                Name = schedule.Name,
                Time = checkDate.Add(schedule.TimeOfDay)
            };
        }
    }
}