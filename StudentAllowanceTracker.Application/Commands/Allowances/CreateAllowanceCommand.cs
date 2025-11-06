using MediatR;
using StudentAllowanceTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class CreateAllowanceCommand: IRequest<Result<AllowanceDTO>>
    {

        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AllowanceType Type { get; set; }

    }
}
