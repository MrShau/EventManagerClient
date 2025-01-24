using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Models
{
    public enum EventTypes
    {
        CONFERENCE,
        SEMINAR,
        TRAINING,
        CORPORATE
    }

    class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EventTypeString { get; set; }
        public int EventType { get; set; }
        public string Participants { get; set; }
        public int ParticipantCount { get; set; }
        
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public EventModel Original { get; set; }
        public bool IsNotParticipant { get; set; } = true;

        public bool HasChanged()
        {
            return Title != Original?.Title || Description != Original?.Description || EventType != Original?.EventType || DateTime != Original?.DateTime || Date != Original?.Date || Time != Original?.Time;
        }
    }
}
