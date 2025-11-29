using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentAllowanceTracker.Application.Commands.Notification;
using MediatR;
using StudentAllowanceTracker.Shared.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

public class DailyReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const int ReminderHourPH = 8; 

    public DailyReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = TimeHelper.TimeUntilNextRun(ReminderHourPH);

            await Task.Delay(delay, stoppingToken);

            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                await mediator.Send(new SendUserReminderCommand());
                Console.WriteLine($"Daily reminders sent at {TimeHelper.Now():yyyy-MM-dd HH:mm:ss} PH time");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending reminders: {ex.Message}");
            }
        }
    }
}
