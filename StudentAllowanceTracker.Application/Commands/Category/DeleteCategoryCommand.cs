using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Category
{
    public class DeleteCategoryCommand : IRequest<object>
    {
        public Guid CategoryID { get; set; }
    }

}
