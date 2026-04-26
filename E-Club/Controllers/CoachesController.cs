namespace E_Club.Controllers;

[Route("api/coaches")]
[ApiController]
[Authorize]
public class CoachesController(ICoachService coachService) : ControllerBase
{
    /// <summary>Get all active coaches</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await coachService.GetAllCoachesAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>Get coach by ID</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await coachService.GetCoachByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>Get coaches by specialization (e.g. Football, Swimming)</summary>
    [HttpGet("specialization/{specialization}")]
    public async Task<IActionResult> GetBySpecialization(string specialization, CancellationToken cancellationToken)
    {
        var result = await coachService.GetCoachesBySpecializationAsync(specialization, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>Create a new coach (Admin only)</summary>
    [HttpPost]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateCoachRequest request, CancellationToken cancellationToken)
    {
        var result = await coachService.CreateCoachAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    /// <summary>Update a coach (Admin only)</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCoachRequest request, CancellationToken cancellationToken)
    {
        var result = await coachService.UpdateCoachAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "Coach updated successfully" }) : result.ToProblem();
    }

    /// <summary>Delete (deactivate) a coach (Admin only)</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await coachService.DeleteCoachAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    /// <summary>Assign a coach to a sport class (Admin only)</summary>
    [HttpPost("classes/{classId:int}/assign/{coachId:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> AssignToClass(int classId, int coachId, CancellationToken cancellationToken)
    {
        var result = await coachService.AssignCoachToClassAsync(classId, coachId, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "Coach assigned successfully" }) : result.ToProblem();
    }

    /// <summary>Remove coach from a sport class (Admin only)</summary>
    [HttpDelete("classes/{classId:int}/assign")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> RemoveFromClass(int classId, CancellationToken cancellationToken)
    {
        var result = await coachService.RemoveCoachFromClassAsync(classId, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "Coach removed from class" }) : result.ToProblem();
    }
}
