namespace E_Club.Controllers;

[Route("api/banners")]
[ApiController]
[Authorize]
public class BannersController(IBannerService bannerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActiveBanners()
    {
        var result = await bannerService.GetActiveBannersAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await bannerService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Create(CreateBannerRequest request)
    {
        var userId = User.GetUserId();
        var result = await bannerService.CreateAsync(request, userId!);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Update(int id, CreateBannerRequest request)
    {
        var result = await bannerService.UpdateAsync(id, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await bannerService.DeleteAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var result = await bannerService.ToggleStatusAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
