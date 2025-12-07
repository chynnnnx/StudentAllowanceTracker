using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAllowanceTracker.Application.Commands.History;
using StudentAllowanceTracker.Application.Queries.History;

[Route("api/[controller]")]
[ApiController]
[Authorize]


public class HistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHistory([FromBody] CreateHistoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetHistoryById), new { id = result.Data!.HistoryID }, result.Data);
    }
         
    [HttpGet]
    public async Task<IActionResult> GetHistory([FromQuery] string? type)
    {
        var query = new GetHistoryQuery { Type = type };
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHistoryById(Guid id)
    {
        var query = new GetHistoryByIdQuery { HistoryID = id };
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHistory(Guid id)
    {
        var command = new DeleteHistoryCommand { HistoryID = id };
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return NoContent();
    }
}
