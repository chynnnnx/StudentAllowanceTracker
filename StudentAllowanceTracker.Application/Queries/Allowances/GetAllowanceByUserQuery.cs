using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Queries.Allowances
{
    public class GetAllowanceByUserQuery: IRequest<List<AllowanceDTO>>
    {
    }
}
