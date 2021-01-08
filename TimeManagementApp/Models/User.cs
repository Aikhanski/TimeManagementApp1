using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagementApp.Models
{
    public class User: IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Company { get; set; }
        public List<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    }
}
