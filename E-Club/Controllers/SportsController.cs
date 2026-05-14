namespace E_Club.Controllers;

[Route("api/sports")]
[ApiController]
[Authorize]
public class SportsController(ISportService sportService) : ControllerBase
{
    [HttpGet("screen")]
    public async Task<IActionResult> GetSportsScreen([FromQuery] int? sportId)
    {
        var userId = User.GetUserId();
        var result = await sportService.GetSportsScreenAsync(sportId, userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSports()
    {
        var result = await sportService.GetAllSportsAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("classes")]
    public async Task<IActionResult> GetUpcomingClasses([FromQuery] int? sportId, [FromQuery] int? coachId, [FromQuery] DateTime? date)
    {
        var userId = User.GetUserId();
        var result = await sportService.GetUpcomingClassesAsync(sportId, userId!, coachId, date);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("special-event")]
    public async Task<IActionResult> GetSpecialEvent([FromQuery] int? sportId)
    {
        var userId = User.GetUserId();
        var result = await sportService.GetSpecialEventAsync(sportId, userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("classes/{id}/book")]
    public async Task<IActionResult> BookClass(int id)
    {
        var userId = User.GetUserId();
        var result = await sportService.BookClassAsync(id, userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("classes/{id}/join")]
    public async Task<IActionResult> JoinSpecialEvent(int id)
    {
        var userId = User.GetUserId();
        var result = await sportService.BookClassAsync(id, userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("classes/{id}/book")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId = User.GetUserId();
        var result = await sportService.CancelBookingAsync(id, userId!);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.GetUserId();
        var result = await sportService.GetMyBookingsAsync(userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("classes")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> CreateClass(CreateSportClassRequest request)
    {
        var userId = User.GetUserId();
        var result = await sportService.CreateClassAsync(request, userId!);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUpcomingClasses), new { }, result.Value) : result.ToProblem();
    }

    [HttpPut("classes/{id}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> UpdateClass(int id, CreateSportClassRequest request)
    {
        var result = await sportService.UpdateClassAsync(id, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("classes/{id}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var result = await sportService.DeleteClassAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
