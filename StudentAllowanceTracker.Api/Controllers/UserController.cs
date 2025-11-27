using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using StudentAllowanceTracker.Application.Commands.User;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id.ToString() != command.Id)
                return BadRequest("User ID mismatch.");
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.Errors);
            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var query = new Application.Queries.User.GetInfoByUserQuery { };
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound("User not found.");
            return Ok(result);
        }

        [HttpPatch("profile/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return BadRequest(result.Errors);
            return NoContent();
        }
    }
}
