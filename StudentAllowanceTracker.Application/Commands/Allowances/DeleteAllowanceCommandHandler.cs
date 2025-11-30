using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Common.Exceptions;
namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class DeleteAllowanceCommandHandler: IRequestHandler<DeleteAllowanceCommand, Result<object> >
    {
        private readonly IBaseRepository<Allowance> _allowanceRepo;
        public DeleteAllowanceCommandHandler(IBaseRepository<Allowance> allowanceRepo)
        {
            _allowanceRepo = allowanceRepo;
        }
        public async Task<Result<object>> Handle(DeleteAllowanceCommand request, CancellationToken cancellationToken)
        {
            var allowance = await _allowanceRepo.GetByIdAsync(request.AllowanceID);
            if (allowance == null)
                return Result<object>.Fail(ResultStatus.NotFound, "Allowance not found or access denied.");

            await _allowanceRepo.DeleteAsync(request.AllowanceID);
            return Result<object>.Ok();
        }
    }
}
