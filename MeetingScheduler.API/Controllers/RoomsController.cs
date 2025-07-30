using MeetingScheduler.Data;
using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.API.Controllers
{
    /// <summary>
    /// Controlador da entidade Room (sala)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomDataService _roomDataService;
        /// <summary>
        /// Construtor do controlador da entidade Room (sala)
        /// </summary>
        /// <param name="roomDataService">Serviço que busca dados da entidade Room (sala)</param>
        public RoomsController(IRoomDataService roomDataService)
        {
            _roomDataService = roomDataService;
        }

        /// <summary>
        /// Obtém todas as salas existentes
        /// </summary>
        /// <returns>Uma lista de todas as salas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRooms()
        {
            var rooms = await _roomDataService.GetAllRooms();

            return Ok(rooms);
        }

    }
}
