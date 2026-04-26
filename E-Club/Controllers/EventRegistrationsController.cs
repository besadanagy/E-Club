namespace E_Club.Controllers;

[Route("api/events")]
[ApiController]
[Authorize]
public class EventRegistrationsController(IEventRegistrationService registrationService) : ControllerBase
{
    [HttpPost("{id}/register")]
    public async Task<IActionResult> Register(int id)
    {
        var userId = User.GetUserId();
        var result = await registrationService.RegisterAsync(id, userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("{id}/register")]
    public async Task<IActionResult> CancelRegistration(int id)
    {
        var userId = User.GetUserId();
        var result = await registrationService.CancelRegistrationAsync(id, userId!);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpGet("my-registrations")]
    public async Task<IActionResult> GetMyRegistrations()
    {
        var userId = User.GetUserId();
        var result = await registrationService.GetMyRegistrationsAsync(userId!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
