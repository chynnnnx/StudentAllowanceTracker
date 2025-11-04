using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class DeleteAllowanceCommand: IRequest
    {
        public Guid AllowanceID { get; set; }
    }
}
