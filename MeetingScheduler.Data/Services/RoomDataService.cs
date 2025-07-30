using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.Data.Services
{
    public class RoomDataService : IRoomDataService
    {
        private readonly MeetingSchedulerContext _context;

        public RoomDataService(MeetingSchedulerContext context)
        {
            _context = context;
        }
        public async Task<Room?> GetRoom(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Rooms.Include(r => r.Meetings).ToArrayAsync();
        }
    }
}
