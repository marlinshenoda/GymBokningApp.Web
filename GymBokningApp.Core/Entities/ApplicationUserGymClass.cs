﻿

namespace GymBokningApp.Core.Entities
{
    #nullable disable
    public class ApplicationUserGymClass
    {
        public int GymClassId { get; set; }
        public GymClass GymClass { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
