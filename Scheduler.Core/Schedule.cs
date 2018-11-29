using System;

namespace Scheduler.Core
{
    /// <summary>
    /// The Schedule class is the base class for all types that implement scheduling rules.
    /// It is abstract, so cannot be instantiated in its own right. However, it defines two concrete properties.
    /// TimeOfDay is set to the time of the appointment; each appointment may only occur once per day.
    /// Name specifies a name for the appointment. This would be useful if using the scheduling library to create calendars. 
    /// </summary>
    public abstract class Schedule : ISchedule
    {
        public TimeOfDay TimeOfDay { get; set; }
 
        public string Name { get; set; }
 
        public abstract bool OccursOnDate(DateTime date);
    }
}