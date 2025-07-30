using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IRoomDataService
    {
        Task<Room?> GetRoom(int id);
        Task<IEnumerable<Room>> GetAllRooms();
    }
}
