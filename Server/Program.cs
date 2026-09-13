using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.OpenApi;
using Server.Application.DTO;
using Server.Application.Handlers;
using Server.Application.Services.EventPublisher;
using Server.Application.Services.EventPublisher.Imp;
using Server.Domain.Events;
using Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddSingleton<EventManagementService>();

builder.Services.AddScoped<IEventHandler<EventCreatedEvent>, LogEventCreatedHandler>();
builder.Services.AddScoped<IEventHandler<ParticipantRegisteredEvent>, SendEmailOnParticipantRegisteredHandler>();
builder.Services.AddScoped<IEventHandler<TicketIssuedEvent>, GenerateQrCodeOnTicketIssuedHandler>();
builder.Services.AddScoped<IEventHandler<RegistrationCancelledEvent>, LogRegistrationCancelledHandler>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapPost("/api/events", async ([FromBody] CreateEventCommand cmd, EventManagementService service) =>
{
    var id = await service.CreateEventAsync(cmd);
    return Results.Created($"/api/events/{id}", new { EventId = id });
});

app.MapPost("/api/registrations", async ([FromBody] RegisterParticipantCommand cmd, EventManagementService service) =>
{
    var id = await service.RegisterParticipantAsync(cmd);
    return Results.Ok(new { RegistrationId = id });
});

app.MapPost("/api/registrations/{id:guid}/cancel", async (Guid id, EventManagementService service) =>
{
    await service.CancelRegistrationAsync(new CancelRegistrationCommand(id));
    return Results.NoContent();
});

app.MapPost("/api/tickets/issue", async ([FromBody] IssueTicketCommand cmd, EventManagementService service) =>
{
    var id = await service.IssueTicketAsync(cmd);
    return Results.Ok(new { TicketId = id });
});

app.Run();

