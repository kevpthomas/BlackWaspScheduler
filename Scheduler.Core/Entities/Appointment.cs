using System;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// The Appointment class holds information about generated appointments and does not add any functionality.
    /// </summary>
    public class Appointment
    {
        public DateTime Time { get; set; }
 
        public string Name { get; set; }
    }

}