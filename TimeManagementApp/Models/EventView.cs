using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagementApp.Models
{
    public class EventView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
    public class EventDetailView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<EventParticipant> Participants { get; set; }
    }
}
