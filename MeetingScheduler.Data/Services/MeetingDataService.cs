using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.Data.Services
{
    public class MeetingDataService : IMeetingDataService
    {
        private readonly MeetingSchedulerContext _context;

        public MeetingDataService(MeetingSchedulerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meeting>> ScheduledMeetingsByRoomAsync(int roomId)
        {
            return await _context.Meetings
                .Where(m => m.Status == MeetingStatus.Scheduled && m.RoomId == roomId)
                .ToArrayAsync();
        }
    }
}
