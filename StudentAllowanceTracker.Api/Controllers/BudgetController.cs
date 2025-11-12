using Microsoft.AspNetCore.Mvc;
using MediatR;
using StudentAllowanceTracker.Application.Commands.Budget;
using StudentAllowanceTracker.Application.Queries.Budget;
using System;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-budget")]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] UpdateBudgetCommand command)
        {
            if (id != command.BudgetID)
                return BadRequest("Budget ID mismatch.");

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgetsByUser()
        {
            var query = new GetBudgetByUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(Guid id)
        {
            var command = new DeleteBudgetCommand { BudgetID = id };
            var result = await _mediator.Send(command);

            return NoContent();
        }
    }
}
