using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.Expense;
using StudentAllowanceTracker.Application.Queries.Expense;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-expense")]
        public async Task< IActionResult> AddExpense([FromBody] CreateExpenseCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task <IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseCommand command)
        {
            if (id != command.ExpenseID)
                return BadRequest("Mismatched Expense ID.");
            var result = await _mediator.Send(command)  ;
            return Ok(result);
        }

        [HttpGet]
        public async Task <IActionResult> GetExpenses()
        {
            var query = new GetExpenseByUserQuery { };
            var expenses = await _mediator.Send(query);
            return Ok(expenses);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            await _mediator.Send(new DeleteExpenseCommand { ExpenseID = id });
            return NoContent();
        }
    }
}
