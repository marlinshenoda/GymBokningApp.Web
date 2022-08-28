using System;
using System.Collections.Generic;

namespace GymBokningApp.Core.Entities
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration; /*{ get { return StartTime + Duration; } }*/
        public string Description { get; set; } = string.Empty;
        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; } = new List<ApplicationUserGymClass>();

    }
}
