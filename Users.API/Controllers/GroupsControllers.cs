using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.APP.Features.Groups;

namespace Users.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class GroupsControllers : ControllerBase
{
    private readonly IMediator _mediator;

    // Constructor ile IMediator dependency injection
    public GroupsControllers(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// Get all groups from the database.
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = await _mediator.Send(new GroupQueryRequest());
        var list = await query.ToListAsync();
        return Ok(list);
    }

    /// Get groups by id from the database.
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = await _mediator.Send(new GroupQueryRequest());
        var item = await query.SingleOrDefaultAsync( groupResponse=> groupResponse.Id == id );
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post(GroupCreateRequest request)
    {
        if (ModelState.IsValid)
        {
            var response = await _mediator.Send(request);

            if (response.IsSuccesful)
            {
                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            return BadRequest(response);
        }
        return BadRequest(ModelState);
    }
    
    
    
}