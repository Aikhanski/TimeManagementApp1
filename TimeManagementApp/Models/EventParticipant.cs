using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagementApp.Models
{
    public class EventParticipant
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
