namespace E_Club.Controllers;

[Route("api/home")]
[ApiController]
[Authorize]
public class HomeController(IHomeService homeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHomeData()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await homeService.GetHomeDataAsync(userId);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
