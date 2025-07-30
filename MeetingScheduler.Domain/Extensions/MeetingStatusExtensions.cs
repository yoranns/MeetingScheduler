using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Extensions
{
    public static class MeetingStatusExtensions
    {
        public static string ToDescriptionString(this MeetingStatus status)
        {
            return status switch
            {
                MeetingStatus.Scheduled => "Scheduled",
                MeetingStatus.Cancelled => "Cancelled",
                _ => status.ToString(),
            };
        }

        public static MeetingStatus FromDescriptionString(string description)
        {
            switch (description.ToLower())
            {
                case "scheduled":
                    return MeetingStatus.Scheduled;
                case "cancelled":
                    return MeetingStatus.Cancelled;
                default:
                    throw new ArgumentException("Invalid meeting status description", nameof(description));
            }
        }
    }
}
