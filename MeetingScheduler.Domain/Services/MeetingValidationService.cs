using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;

namespace MeetingScheduler.Domain.Services
{
    public class MeetingValidationService : IMeetingValidationService
    {
        public bool ValidateMeeting(Meeting meeting, out string errorMessage)
        {
            errorMessage = string.Empty;
            var isValid = true;

            var duration = meeting.EndTime - meeting.StartTime;
            if (duration < TimeSpan.FromMinutes(30))
            {
                errorMessage = "- A duração da reunião não pode ser menor que 30 minutos.";
                isValid = false;
            }
            else if (duration > TimeSpan.FromHours(4))
            {
                errorMessage = "- A duração da reunião não pode ser maior que 4 horas.";
                isValid = false;
            }

            if (!string.IsNullOrEmpty(errorMessage)) errorMessage += "\n";

            if (meeting.ParticipantCount <= 0)
            {
                errorMessage += "- Número de participantes deve ser maior que 0.";
                isValid = false;
            }
            else if (meeting.ParticipantCount > meeting.Room?.Capacity)
            {
                errorMessage += "- Número de participantes excede a capacidade da sala.";
                isValid = false;
            }

            return isValid;
        }
    }
}
