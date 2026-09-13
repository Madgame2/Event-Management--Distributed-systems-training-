using FluentAssertions;
using Moq;
using Server.Application.DTO;
using Server.Application.Services.EventPublisher;
using Server.Domain.Events;
using Server.Domain.Exceptions;
using Xunit;

namespace Tests;

public class EventManagementServiceTests
{
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly EventManagementService _service;
    
    public EventManagementServiceTests()
    {
        _eventBusMock = new Mock<IEventBus>();
        _service = new EventManagementService(_eventBusMock.Object);
    }
    
    [Fact]
    public async Task CreateEvent_WithValidData_ShouldCreateEventAndPublishEvent()
    {
        // Arrange (Подготовка данных)
        var command = new CreateEventCommand(
            Title: "Tech Conference 2026",
            StartDate: DateTime.UtcNow.AddDays(10),
            VenueId: Guid.NewGuid(),
            MaxCapacity: 100
        );

        // Act (Выполнение действия)
        var eventId = await _service.CreateEventAsync(command);

        // Assert (Проверка результатов)
        eventId.Should().NotBeEmpty();
        
        _eventBusMock.Verify(bus => bus.PublishAsync(
            It.Is<EventCreatedEvent>(e => e.EventId == eventId && e.Title == command.Title)
        ), Times.Once);
    }
    
    
    [Fact]
    public async Task RegisterParticipant_WhenEventDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistingEventId = Guid.NewGuid();
        var command = new RegisterParticipantCommand(
            EventId: nonExistingEventId,
            ParticipantId: Guid.NewGuid()
        );

        // Act
        Func<Task> act = async () => await _service.RegisterParticipantAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Мероприятие с ID {nonExistingEventId} не найдено.");

        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<ParticipantRegisteredEvent>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateEvent_WithInvalidMaxCapacity_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateEventCommand(
            Title: "Invalid Event",
            StartDate: DateTime.UtcNow.AddDays(5),
            VenueId: Guid.NewGuid(),
            MaxCapacity: -10 // Ошибка: вместимость должна быть > 0
        );

        // Act
        Func<Task> act = async () => await _service.CreateEventAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Вместимость мероприятия должна быть больше 0.");

        _eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<EventCreatedEvent>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateEvent_WhenEventBusFails_ShouldThrowNetworkCommunicationException()
    {
        // Arrange
        var command = new CreateEventCommand(
            Title: "Event with Network Issue",
            StartDate: DateTime.UtcNow.AddDays(10),
            VenueId: Guid.NewGuid(),
            MaxCapacity: 50
        );

        _eventBusMock
            .Setup(bus => bus.PublishAsync(It.IsAny<EventCreatedEvent>()))
            .ThrowsAsync(new NetworkCommunicationException("Сервер сообщений недоступен или произошла ошибка сети."));

        // Act
        Func<Task> act = async () => await _service.CreateEventAsync(command);

        // Assert
        await act.Should().ThrowAsync<NetworkCommunicationException>()
            .WithMessage("Сервер сообщений недоступен или произошла ошибка сети.");
    }
}