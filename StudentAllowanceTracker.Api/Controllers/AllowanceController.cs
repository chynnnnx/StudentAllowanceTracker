using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.Allowances;
using StudentAllowanceTracker.Application.Queries.Allowances;
using System.Security.Claims;

namespace StudentAllowanceTracker.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AllowanceController : ControllerBase
    {
        private readonly  IMediator _mediator;

        public AllowanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-allowance")]
        public async Task <IActionResult>  AddAllowance([FromBody] CreateAllowanceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAllowance(Guid id, [FromBody] UpdateAllowanceCommand command)
        {
            if (id != command.AllowanceID)
                return BadRequest("Mismatched Allowance ID.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllowanceByUser()
        {

            var query = new GetAllowanceByUserQuery { };
            var allowances = await _mediator.Send(query);

            return Ok(allowances);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllowance(Guid id)
        {
            await _mediator.Send(new DeleteAllowanceCommand { AllowanceID = id });
            return NoContent(); 
        }

    }
}
