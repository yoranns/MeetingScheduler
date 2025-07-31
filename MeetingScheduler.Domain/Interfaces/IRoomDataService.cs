using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Interfaces
{
    public interface IRoomDataService
    {
        Task<Room?> GetRoomAsync(int id);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
    }
}
