
using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IMeetingDataService
    {
        Task AddMeeting(Meeting meeting);
        Task UpdateMeeting(Meeting meeting);
        Task<Meeting?> GetMeetingAsync(int id);
        Task<IEnumerable<Meeting>> GetScheduledMeetingsByRoomAsync(int roomId);
        Task<List<Meeting>> GetScheduledMeetingsByDate(DateTime date);
    }
}
