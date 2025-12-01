using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.Auth.LoginAndRegister;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Email verified successfully." });
        }
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success) return BadRequest(result.Errors);

            return Ok(result.Data);
        }


    }
}
