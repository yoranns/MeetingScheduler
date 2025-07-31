using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IRoomValidationService
    {
        bool IsRoomAvailable(DateTime startTime, DateTime endTime, IEnumerable<Meeting> scheduledMeetings, out string errorMesseage);
    }
}
