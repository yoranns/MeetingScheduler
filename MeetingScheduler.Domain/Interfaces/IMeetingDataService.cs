
using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IMeetingDataService
    {
        Task<IEnumerable<Meeting>> ScheduledMeetingsByRoomAsync(int roomId);
    }
}
