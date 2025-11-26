using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.Commands.History;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;

public static class HistoryHelper
{
    /// <summary>
    /// Automatically logs a record to History table.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity being logged</typeparam>
    public static async Task LogAsync<TEntity>( TEntity entity,string type, IMapper mapper, IMediator mediator)
    {
        if (entity == null) return;

        var historyCommand = mapper.Map<CreateHistoryCommand>(entity);
        historyCommand.Type = type;
        historyCommand.Date = DateTime.UtcNow;

        await mediator.Send(historyCommand);
    }
}
