using ConsoleClinet.Services;
using Server.Application.DTO;

namespace ConsoleClinet.UI;

public class EventConsoleMenu
{
    private readonly IEventApiClient _apiClient;

    public EventConsoleMenu(IEventApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task RunAsync()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            ConsoleRenderer.DrawHeader();
            ConsoleRenderer.DrawMenu();

            var choice = ConsoleRenderer.ReadInput("Выберите действие");

            if (choice == "0")
            {
                ConsoleRenderer.PrintInfo("Завершение сеанса...");
                break;
            }

            try
            {
                switch (choice)
                {
                    case "1":
                        await HandleCreateEventAsync();
                        break;
                    case "2":
                        await HandleRegisterParticipantAsync();
                        break;
                    default:
                        ConsoleRenderer.PrintError("Неизвестный пункт меню. Попробуйте снова.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleRenderer.PrintError(ex.Message);
            }

            ConsoleRenderer.PrintInfo("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
    
    private async Task HandleCreateEventAsync()
    {
        ConsoleRenderer.PrintSectionHeader("Создание мероприятия");

        var title = ConsoleRenderer.ReadInput("Название мероприятия");
        
        if (!int.TryParse(ConsoleRenderer.ReadInput("Через сколько дней начало"), out int days))
        {
            ConsoleRenderer.PrintError("Количество дней должно быть числом.");
            return;
        }

        if (!int.TryParse(ConsoleRenderer.ReadInput("Максимальная вместимость"), out int capacity))
        {
            ConsoleRenderer.PrintError("Вместимость должна быть числом.");
            return;
        }

        var command = new CreateEventCommand(
            Title: title,
            StartDate: DateTime.UtcNow.AddDays(days),
            VenueId: Guid.NewGuid(),
            MaxCapacity: capacity
        );

        ConsoleRenderer.PrintInfo("Формирование и отправка запроса на сервер...");
        var result = await _apiClient.CreateEventAsync(command);
        ConsoleRenderer.PrintSuccess(result);
    }
    
    private async Task HandleRegisterParticipantAsync()
    {
        ConsoleRenderer.PrintSectionHeader("Регистрация участника");

        var rawEventId = ConsoleRenderer.ReadInput("ID мероприятия (GUID)");
        if (!Guid.TryParse(rawEventId, out var eventId))
        {
            ConsoleRenderer.PrintError("Неверный формат GUID для ID мероприятия.");
            return;
        }

        var rawParticipantId = ConsoleRenderer.ReadInput("ID участника (GUID)");
        if (!Guid.TryParse(rawParticipantId, out var participantId))
        {
            ConsoleRenderer.PrintError("Неверный формат GUID для ID участника.");
            return;
        }

        var command = new RegisterParticipantCommand(
            EventId: eventId,
            ParticipantId: participantId
        );

        ConsoleRenderer.PrintInfo("Формирование и отправка запроса на сервер...");
        var result = await _apiClient.RegisterParticipantAsync(command);
        ConsoleRenderer.PrintSuccess(result);
    }
}