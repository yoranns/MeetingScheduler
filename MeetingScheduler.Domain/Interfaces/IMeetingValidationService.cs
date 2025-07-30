using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IMeetingValidationService
    {
        bool ValidateMeeting(Meeting meeting, out string errorMessage);

        bool ValidateMeetingCancellation(Meeting meeting, out string errorMessage);
    }
}
