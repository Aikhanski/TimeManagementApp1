using System;
using System.Collections.Generic;

namespace TimeManagementApp.Models
{
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Event(User creator)
        {
            Id = Guid.NewGuid().ToString();
            Creator = creator;
        }
        public string Id { get; set; }
        public User Creator { get; set; }
        public List<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
        public EventContent Content { get; set; }
    }
}
