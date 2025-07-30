namespace MeetingScheduler.Domain.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; } = null;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ParticipantCount { get; set; }
        public string Organizer { get; set; }
        public MeetingStatus Status { get; set; } = MeetingStatus.Scheduled;
    }

    public enum MeetingStatus
    {
        Cancelled = 0,
        Scheduled
    }
}
