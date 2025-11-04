using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class DeleteAllowanceCommandHandler: IRequestHandler<DeleteAllowanceCommand>
    {
        private readonly IBaseRepository<Allowance> _allowanceRepo;
        public DeleteAllowanceCommandHandler(IBaseRepository<Allowance> allowanceRepo)
        {
            _allowanceRepo = allowanceRepo;
        }

        public async Task Handle(DeleteAllowanceCommand request, CancellationToken cancellationToken)
        {
            var allowance = await _allowanceRepo.GetByIdAsync(request.AllowanceID)
                          ?? throw new Exception("Allowance not found.");

            await _allowanceRepo.DeleteAsync(request.AllowanceID);


        }
    }
}
