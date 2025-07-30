using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingScheduler.Domain.Services
{
    public class RoomValidationService : IRoomValidationService
    {
        public bool IsRoomAvailable(int roomId, DateTime startTime, DateTime endTime, IEnumerable<Meeting> scheduledMeetings, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Condição 1: Verifica se a nova reunião começa enquanto uma reunião existente ainda está em andamento.
            // Condição 2: Verifica se a nova reunião termina enquanto uma reunião existente ainda está ocorrendo.
            // Condição 3: Verifica se a nova reunião está completamente contida dentro do intervalo de uma reunião existente.
            // Dessa forma é possível cobrir todos os cenários possíveis de sobreposição de horários.
            var valid = !scheduledMeetings.Any(meeting =>
                (meeting.StartTime <= startTime && meeting.EndTime > startTime) ||
                (meeting.StartTime < endTime && meeting.EndTime >= endTime) ||
                (startTime >= meeting.StartTime && endTime <= meeting.EndTime));

            if (!valid)
            {
                errorMessage = "- A sala não está disponível no horário solicitado.";
            }

            return valid;
        }
    }
}
