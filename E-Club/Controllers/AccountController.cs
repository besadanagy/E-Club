
namespace E_Club.Controllers
{
    [Route("account")]  // تغيير لمسار واضح
    [ApiController]
    [Authorize]  // تأكد من وجودها
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _userService.GetProfileAsync(userId);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _userService.UpdateProfileAsync(userId, request);

            return result.IsSuccess
                ? Ok(new { message = "Profile updated successfully" })
                : result.ToProblem();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _userService.ChangePasswordAsync(userId, request);

            return result.IsSuccess
                ? Ok(new { message = "Password changed successfully" })
                : result.ToProblem();
        }
    }
}