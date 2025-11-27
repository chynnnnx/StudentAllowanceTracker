using StudentAllowanceTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace StudentAllowanceTracker.Application.Queries.User
{
    public class GetInfoByUserQuery: IRequest<AuthUserDTO>
    {
    }
}
