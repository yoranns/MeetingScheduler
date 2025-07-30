using MeetingScheduler.Data;
using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
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
        private readonly MeetingSchedulerContext _context;
        private readonly IMeetingValidationService _meetingValidationService;
        /// <summary>
        /// Construtor do controlador da entidade Meeting (reunião)
        /// </summary>
        /// <param name="context">Contexto de agendamento de reuinão que permite I/0 no BD</param>
        /// <param name="meetingValidationService">Serviço que permite validação da entidade Meeting (reunião) </param>
        public MeetingsController(MeetingSchedulerContext context, IMeetingValidationService meetingValidationService)
        {
            _context = context;
            _meetingValidationService = meetingValidationService;
        }

        /// <summary>
        /// Retorna a reuinão identificada pelo id informado
        /// </summary>
        /// <param name="id">Id da reunião</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
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
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Meeting>> ScheduleMeeting(Meeting meeting)
        {
            if (!_meetingValidationService.ValidateMeeting(meeting, out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            _context.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

    }
}
