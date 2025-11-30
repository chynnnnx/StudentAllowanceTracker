using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.Interfaces;
using MediatR;
using AutoMapper;
using StudentAllowanceTracker.Application.Commands.History;
namespace StudentAllowanceTracker.Application.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HistoryService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task LogAsync<TEntity>(TEntity entity, string type)
        { if (entity == null) return;

            var history = _mapper.Map<CreateHistoryCommand>(entity);
            history.Type = type;
            history.Date = DateTime.UtcNow;

            await _mediator.Send(history);


        }
    }
}
