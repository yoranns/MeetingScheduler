
using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IMeetingDataService
    {
        Task AddMeetingAsync(Meeting meeting);
        Task UpdateMeetingAsync(Meeting meeting);
        Task<Meeting?> GetMeetingAsync(int id);
        Task<IEnumerable<Meeting>> GetScheduledMeetingsByRoomAsync(int roomId);
        Task<List<Meeting>> GetScheduledMeetingsByDateAsync(DateTime date);
    }
}
