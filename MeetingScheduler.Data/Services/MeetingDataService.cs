using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeetingScheduler.Data.Services
{
    public class MeetingDataService : IMeetingDataService
    {
        private readonly MeetingSchedulerContext _context;

        public MeetingDataService(MeetingSchedulerContext context)
        {
            _context = context;
        }

        public async Task AddMeeting(Meeting meeting)
        {
            _context.Add(meeting);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMeeting(Meeting meeting)
        {
            _context.Update(meeting);
            await _context.SaveChangesAsync();
        }
        public async Task<Meeting?> GetMeetingAsync(int id)
        {
           return await _context.Meetings.
                    Include(m => m.Room).
                    FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Meeting>> GetScheduledMeetingsByRoomAsync(int roomId)
        {
            return await _context.Meetings
                .Where(m => m.Status == MeetingStatus.Scheduled && m.RoomId == roomId)
                .ToArrayAsync();
        }

        public async Task<List<Meeting>> GetScheduledMeetingsByDate(DateTime date)
        {
            return await _context.Meetings
                     .Include(m => m.Room)
                     .Where(m => m.Status == MeetingStatus.Scheduled &&
                                 m.StartTime.Date == date.Date)
                     .ToListAsync();
        }
    }
}
