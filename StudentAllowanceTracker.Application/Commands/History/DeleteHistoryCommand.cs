using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.History
{
    public class DeleteHistoryCommand: IRequest<Result<object>>
    {
        public Guid HistoryID { get; set; }
    
    }
}
