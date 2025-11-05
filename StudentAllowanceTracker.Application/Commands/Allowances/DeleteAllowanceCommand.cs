using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class DeleteAllowanceCommand: IRequest<Result<object>>
    {
        public Guid AllowanceID { get; set; }
    }
}
