using MeetingScheduler.Data;
using MeetingScheduler.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.API.Controllers
{
    /// <summary>
    /// Controlador da entidade Room
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly MeetingSchedulerContext _context;
        /// <summary>
        /// Construtor do controlador da entidade Room
        /// </summary>
        /// <param name="context">Contexto de agendamento de reuinão que permite I/0 no BD</param>
        public RoomsController(MeetingSchedulerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as salas existentes
        /// </summary>
        /// <returns>Uma lista de todas as salas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

    }
}
