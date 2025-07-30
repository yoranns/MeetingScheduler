using MeetingScheduler.Data;
using MeetingScheduler.Data.Services;
using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.API.Controllers
{
    /// <summary>
    /// Controlador da entidade Meeting (reunião)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingSchedulerContext _context;
        private readonly IMeetingValidationService _meetingValidationService;
        private readonly IMeetingDataService _meetingDataService;
        private readonly IRoomValidationService _roomValidationService;
        /// <summary>
        /// Construtor do controlador da entidade Meeting (reunião)
        /// </summary>
        /// <param name="context">Contexto de agendamento de reuinão que permite I/0 no BD</param>
        /// <param name="meetingDataService">Serviço que busca dados da entidade Meeting (reunião)</param>
        /// <param name="meetingValidationService">Serviço que permite validação da entidade Meeting (reunião)</param>
        /// <param name="roomValidationService">Serviço que permite validação da entidade Room (sala)</param>
        public MeetingsController(MeetingSchedulerContext context, IMeetingValidationService meetingValidationService, IMeetingDataService meetingDataService,
            IRoomValidationService roomValidationService)
        {
            _context = context;
            _meetingValidationService = meetingValidationService;
            _meetingDataService = meetingDataService;
            _roomValidationService = roomValidationService;
        }

        /// <summary>
        /// Retorna a reuinão identificada pelo id informado
        /// </summary>
        /// <param name="id">Id da reunião</param>
        /// <returns>A reunião obtida</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings.
                Include(m => m.Room).
                FirstOrDefaultAsync(m => m.Id == id);

            if (meeting == null)
            {
                return NotFound();
            }

            return meeting;
        }

        /// <summary>
        /// Registra uma reunião caso cumpra a validação
        /// </summary>
        /// <param name="meeting">Dados da reunião a registrar</param>
        /// <returns>A reunião inserida</returns>
        [HttpPost]
        public async Task<ActionResult<Meeting>> ScheduleMeeting(Meeting meeting)
        {
            meeting.Room = _context.Rooms.FindAsync(meeting.RoomId).Result;
            if (!_meetingValidationService.ValidateMeeting(meeting, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }
            
            var scheduledMeetings = await _meetingDataService.ScheduledMeetingsByRoomAsync(meeting.RoomId);
            if (!_roomValidationService.IsRoomAvailable(meeting.RoomId, meeting.StartTime, meeting.EndTime, scheduledMeetings))
            {
                return BadRequest("- A sala não está disponível no horário solicitado.");
            }

            _context.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

        /// <summary>
        /// Retorna as reuniões agendadas para uma data específica
        /// </summary>
        /// <param name="date">Data para buscar as reuniões agendadas (formato: yyyy-MM-dd)</param>
        /// <returns>Uma lista de reuniões agendadas da data especificada</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetScheduledMeetingsByDate([FromQuery] DateTime date)
        {
            var scheduledMeetings = await _context.Meetings
                .Include(m => m.Room)
                .Where(m => m.Status == MeetingStatus.Scheduled &&
                            m.StartTime.Date == date.Date)
                .ToListAsync();

            // Para garantir que não venha registros incompletos por conta da configuração IgnoreCycles no Program.cs
            scheduledMeetings.ForEach(m => m.Room.Meetings = null);

            return Ok(scheduledMeetings);
        }

        /// <summary>
        /// Cancela a reunião identificada pelo id informado
        /// </summary>
        /// <param name="id">Id da reunião</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            if(!_meetingValidationService.ValidateMeetingCancellation(meeting, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            meeting.Status = MeetingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content para indicar que a operação foi bem-sucedida
        }
    }
}
