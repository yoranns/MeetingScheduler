using MeetingScheduler.Domain.Interfaces;
using MeetingScheduler.Domain.Models;
using MeetingScheduler.Domain.Models.DTOs;
using MeetingScheduler.Domain.Services;
using Moq;

namespace MeetingScheduler.Test
{
    [TestFixture]
    public class MeetingServiceTests
    {
        [Test]
        public async Task ScheduleMeeting_RoomUnavailable_ReturnsFalse()
        {
            // Arrange
            var mockMeetingDataService = new Mock<IMeetingDataService>();
            var roomValidationService = new RoomValidationService();

            mockMeetingDataService.Setup(x =>  x.GetScheduledMeetingsByRoomAsync(It.IsAny<int>()))
                             .ReturnsAsync(new List<Meeting>
                             {
                                 new Meeting { StartTime = new DateTime(2023, 10, 1, 9, 0, 0), EndTime = new DateTime(2023, 10, 1, 11, 0, 0) }
                             });

            // Act
            bool isAvailable = roomValidationService.IsRoomAvailable(new DateTime(2023, 10, 1, 10, 0, 0), new DateTime(2023, 10, 1, 12, 0, 0), mockMeetingDataService.Object.GetScheduledMeetingsByRoomAsync(1).Result, out _);

            // Assert
            Assert.IsFalse(isAvailable);
        }

        [Test]
        public async Task ScheduleMeeting_RoomAvailable_ReturnsTrue()
        {
            // Arrange
            var mockMeetingService = new Mock<IMeetingDataService>();
            mockMeetingService.Setup(x => x.GetScheduledMeetingsByRoomAsync(It.IsAny<int>()))
                             .ReturnsAsync(new List<Meeting>());

            var roomValidationService = new RoomValidationService();

            // Act
            bool isAvailable = roomValidationService.IsRoomAvailable(new DateTime(2023, 10, 1, 10, 0, 0), new DateTime(2023, 10, 1, 12, 0, 0), mockMeetingService.Object.GetScheduledMeetingsByRoomAsync(1).Result, out _);

            // Assert
            Assert.IsTrue(isAvailable);
        }

        [Test]
        public async Task ScheduleMeeting_ValidParticipantCount_ShouldSucceed()
        {
            var mockRoom = new Room
            {
                Id = 1,
                Name = "Sala Alpha",
                Capacity = 30
            };

            var meetingService = new MeetingValidationService();

            var meeting = new Meeting
            {
                Title = "Reunião de Projeto",
                RoomId = 1,
                Room = mockRoom,
                StartTime = new DateTime(3000, 10, 1, 10, 0, 0),
                EndTime = new DateTime(3000, 10, 1, 12, 0, 0),
                ParticipantCount = 20, // Número de participantes é menor que a capacidade da sala
                Organizer = "organizador@example.com"
            };

            // Act
            bool canSchedule = meetingService.ValidateMeeting(meeting, out _);

            // Assert
            Assert.IsTrue(canSchedule);
        }

        [Test]
        public async Task ScheduleMeeting_ValidParticipantCount_ShouldFail()
        {
            var mockRoom = new Room
            {
                Id = 1,
                Name = "Sala Alpha",
                Capacity = 30
            };

            var meetingService = new MeetingValidationService();

            var meeting = new Meeting
            {
                Title = "Reunião de Projeto",
                RoomId = 1,
                Room = mockRoom,
                StartTime = new DateTime(3000, 10, 1, 10, 0, 0),
                EndTime = new DateTime(3000, 10, 1, 12, 0, 0),
                ParticipantCount = 35, // Número de participantes é maior que a capacidade da sala
                Organizer = "organizador@example.com"
            };

            // Act
            bool canSchedule = meetingService.ValidateMeeting(meeting, out _);

            // Assert
            Assert.IsFalse(canSchedule);
        }
    }
}