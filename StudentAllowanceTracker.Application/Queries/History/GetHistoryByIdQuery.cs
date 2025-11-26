using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Queries.History
{
    public class GetHistoryByIdQuery:IRequest<Result<HistoryDTO>>
    {
        public Guid HistoryID { get; set; }

}
}
