using MeetingScheduler.Data;
using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using MeetingScheduler.Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MeetingScheduler.API.Controllers
{
    /// <summary>
    /// Controlador da entidade Meeting (reunião)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingDataService _meetingDataService;
        private readonly IMeetingValidationService _meetingValidationService;
        private readonly IRoomDataService _roomDataService;
        private readonly IRoomValidationService _roomValidationService;
        
        /// <summary>
        /// Construtor do controlador da entidade Meeting (reunião)
        /// </summary>
        /// <param name="meetingDataService">Serviço que busca dados da entidade Meeting (reunião)</param>
        /// <param name="meetingValidationService">Serviço que permite validação da entidade Meeting (reunião)</param>
        /// <param name="roomDataService">Serviço que busca dados da entidade Room (sala)</param>
        /// <param name="roomValidationService">Serviço que permite validação da entidade Room (sala)</param>
        public MeetingsController(IMeetingDataService meetingDataService, IMeetingValidationService meetingValidationService, 
            IRoomDataService roomDataService, IRoomValidationService roomValidationService)
        {
            _meetingDataService = meetingDataService;
            _meetingValidationService = meetingValidationService;
            _roomDataService = roomDataService;
            _roomValidationService = roomValidationService;
        }

        /// <summary>
        /// Retorna a reuinão identificada pelo id informado
        /// </summary>
        /// <param name="id">Id da reunião</param>
        /// <returns>A reunião obtida</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeetingAsync(int id)
        {
            var meeting = await _meetingDataService.GetMeetingAsync(id);

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
        public async Task<ActionResult<Meeting>> ScheduleMeetingAsync(MeetingInputDTO meeting)
        {
            var meetingModel = new Meeting
            {
                Title = meeting.Title,
                RoomId = meeting.RoomId,
                Room = await _roomDataService.GetRoomAsync(meeting.RoomId),
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                ParticipantCount = meeting.ParticipantCount,
                Organizer = meeting.Organizer
            };

            if (!_meetingValidationService.ValidateMeeting(meetingModel, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }
            
            var scheduledMeetings = await _meetingDataService.GetScheduledMeetingsByRoomAsync(meeting.RoomId);
            if (!_roomValidationService.IsRoomAvailable(meeting.StartTime, meeting.EndTime, scheduledMeetings, out var errorMesseage))
            {
                return BadRequest(errorMesseage);
            }

            await _meetingDataService.AddMeetingAsync(meetingModel);
            
            var actionResult = CreatedAtAction(nameof(GetMeetingAsync), new { id = meetingModel.Id, setRoomMeetingsNull = true }, meetingModel);

            // Para garantir que não venha registros incompletos por conta da configuração IgnoreCycles no Program.cs
            meetingModel.Room.Meetings = null;

            return actionResult;
        }

        /// <summary>
        /// Retorna as reuniões agendadas para uma data específica
        /// </summary>
        /// <param name="date">Data para buscar as reuniões agendadas (formato: yyyy-MM-dd)</param>
        /// <returns>Uma lista de reuniões agendadas da data especificada</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetScheduledMeetingsByDateAsync([FromQuery] DateTime date)
        {
            var scheduledMeetings = await _meetingDataService.GetScheduledMeetingsByDateAsync(date);

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
        public async Task<IActionResult> CancelMeetingAsync(int id)
        {
            var meeting = await _meetingDataService.GetMeetingAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            if(!_meetingValidationService.ValidateMeetingCancellation(meeting, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            meeting.Status = MeetingStatus.Cancelled;
            await _meetingDataService.UpdateMeetingAsync(meeting);

            return Ok();
        }
    }
}
