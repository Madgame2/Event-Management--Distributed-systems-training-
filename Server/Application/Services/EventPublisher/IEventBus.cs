namespace Server.Application.Services.EventPublisher;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}