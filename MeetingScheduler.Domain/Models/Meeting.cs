using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingScheduler.Domain.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ParticipantCount { get; set; }
        public string Organizer { get; set; }
        public MeetingStatus Status { get; set; }
    }

    public enum MeetingStatus
    {
        Scheduled,
        Cancelled
    }
}
