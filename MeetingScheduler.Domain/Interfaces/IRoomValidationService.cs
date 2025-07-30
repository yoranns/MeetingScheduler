using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IRoomValidationService
    {
        bool IsRoomAvailable(int roomId, DateTime startTime, DateTime endTime, IEnumerable<Meeting> scheduledMeetings);
    }
}
