using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.Expense;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
