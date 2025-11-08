using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using StudentAllowanceTracker.Application.Commands.Goals;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentAllowanceTracker.Application.Queries.Goal;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GoalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-goal")]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalCommand command)
        {
            if (id != command.GoalID)
                return BadRequest("Mismatched Goal ID.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            await _mediator.Send(new DeleteGoalCommand { GoalID = id });
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetGoalsByUser()
        {
           var query = new GetGoalByUserQuery { };
            var goals = await _mediator.Send(query);
            return Ok(goals);
        }
    }
}
