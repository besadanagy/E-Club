namespace E_Club.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
    [FromBody] LoginRequest request,  // ◀️ مهم
    CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GetRefreshTokenAsync(
                request.Token,
                request.RefreshToken,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RevokeRefreshTokenAsync(
                request.Token,
                request.RefreshToken,
                cancellationToken);

            return result.IsSuccess
                ? Ok(new { message = "Token revoked successfully" })
                : result.ToProblem();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RegisterAsync(request, cancellationToken);

            return result.IsSuccess
                ? Ok(new { message = "Registration successful. Please check your email for confirmation." })
                : result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromBody] ConfirmEmailRequest request)
        {
            var result = await _authenticationService.ConfirmEmailAsync(request);

            return result.IsSuccess
                ? Ok(new { message = "Email confirmed successfully" })
                : result.ToProblem();
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmationEmailAsync(
            [FromBody] ResendConfirmationEmailRequest request)
        {
            var result = await _authenticationService.ResendConfirmationEmailAsync(request);

            return result.IsSuccess
                ? Ok(new { message = "Confirmation email sent" })
                : result.ToProblem();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(
     [FromBody] ForgotPasswordRequest request)
        {
            var result = await _authenticationService.SendResetPasswordCodeAsync(request);
            return result.IsSuccess
                ? Ok(new { message = "Reset code sent successfully" })
                : result.ToProblem();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromBody] ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);

            return result.IsSuccess
                ? Ok(new { message = "Password reset successfully" })
                : result.ToProblem();
        }
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode(VerifyResetCodeRequest request)
        {
            var result = await _authenticationService.VerifyResetCodeAsync(request);
            return result.IsSuccess
                ? Ok(new { message = "Code verified successfully" })
                : result.ToProblem();
        }

        [HttpPost("resend-reset-code")]
        public async Task<IActionResult> ResendResetCode(ResendResetCodeRequest request)
        {
            var result = await _authenticationService.ResendResetCodeAsync(request);
            return result.IsSuccess
                ? Ok(new { message = "Code resent successfully" })
                : result.ToProblem();
        }
    }
}