using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeManagementApp.Models
{
    public class EventContent
    {
        public EventContent()
        {
        }
        public EventContent(string eventId, string title, DateTime date, Event _event, string description, string address, string phone)
        {
            Id = eventId;
            Title = title;
            EventDate = date;
            Event = _event;
            Description = description;
            Address = address;
            Phone = phone;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
    }
    public class EventForm
    {
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class EventEditForm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
