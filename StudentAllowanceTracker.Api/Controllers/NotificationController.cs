using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.Notification;
using StudentAllowanceTracker.Application.Queries.Notification;

namespace StudentAllowanceTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("send-reminder")]
        public async Task<IActionResult> SendReminder()
        {
            var result = await _mediator.Send(new SendUserReminderCommand());
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }
        [HttpPost("subscribe-reminder")]
        public async Task<IActionResult> SubscribeReminder([FromBody] SubscribeReminderCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success) return BadRequest(result.Errors);
            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentSubscription()
        {
            var query = new GetCurrentSubscriptionQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
