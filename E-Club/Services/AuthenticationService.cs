namespace E_Club.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly JwtOptions _jwtOptions;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ApplicationDbContext _context;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtProvider jwtProvider,
            IOptions<JwtOptions> jwtOptions,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthenticationService> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtProvider = jwtProvider;
            _jwtOptions = jwtOptions.Value;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = null;

            if (request.IsAdmin)
            {
                // Admin login
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.ClubCode))
                    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

                user = await _userManager.FindByEmailAsync(request.Email);
                if (user?.ClubCode != request.ClubCode)
                    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }
            else
            {
                // Member login
                if (string.IsNullOrEmpty(request.MembershipId) || string.IsNullOrEmpty(request.SequenceNumber))
                    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

                user = await _userManager.Users
          .FirstOrDefaultAsync(u => u.MembershipId == request.MembershipId
              && u.SequenceNumber == request.SequenceNumber, cancellationToken);
            }

            if (user == null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

            if (result.Succeeded)
            {
                var (userRoles, userPermissions) = await GetUserRolesAndPermission(user, cancellationToken);
                var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

                user.RefreshTokens ??= new List<RefreshToken>();
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration,
                    CreatedOn = DateTime.UtcNow
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email!,
                    token,
                    expiresIn,
                    refreshToken,
                    refreshTokenExpiration,
                    user.MembershipId,
                    user.DigitalAccessKey
                );

                return Result.Success(response);
            }

            var error = result.IsLockedOut ? UserErrors.LockedUser
                : result.IsNotAllowed ? UserErrors.EmailNotConfirmed
                : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(UserErrors.LockedUser);

            // ◀️ نضيف ديكودينج هنا
            var decodedRefreshToken = Uri.UnescapeDataString(refreshToken);

            var userRefreshToken = user.RefreshTokens?.SingleOrDefault(x =>
                x.Token == refreshToken || x.Token == decodedRefreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (userRoles, userPermissions) = await GetUserRolesAndPermission(user, cancellationToken);

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration,
                CreatedOn = DateTime.UtcNow
            });

            await _userManager.UpdateAsync(user);

            return Result.Success(new AuthResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                newToken,
                expiresIn,
                newRefreshToken,
                refreshTokenExpiration,
                user.MembershipId,
                user.DigitalAccessKey
            ));
        }
        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens?.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidCredentials);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result.Failure(UserErrors.InvalidCredentials);

            return Result.Success();
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            // Check if email exists
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
                return Result.Failure(UserErrors.DuplicatedEmail);

            // تقسيم الاسم الكامل
            var nameParts = request.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : "";
            var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

            var user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                NationalId = request.NationalId,      // ◀️ إضافة National ID
                UserName = request.Email,
                CreatedOn = DateTime.UtcNow
            };

            if (!request.IsAdmin)
            {
                // Generate Membership ID and Sequence Number للعضو العادي
                user.MembershipId = GenerateMembershipId();
                user.SequenceNumber = GenerateSequenceNumber();
                user.DigitalAccessKey = GenerateDigitalAccessKey();
            }
            else
            {
                // Admin registration
                user.ClubCode = GenerateClubCode();  // ◀️ محتاجين دالة جديدة
            }

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Add to default role
                await _userManager.AddToRoleAsync(user, request.IsAdmin ? DefaultRoles.Admin : DefaultRoles.Member);

                // Send confirmation email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                SendConfirmationEmail(user, code);

                return Result.Success();
            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentials);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailIsConfirmed);

            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Result.Success();
            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailIsConfirmed);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Email Confirmation Code : {code}", code);

            SendConfirmationEmail(user, code);

            return Result.Success();
        }

        public async Task<Result> SendResetPasswordCodeAsync(ForgotPasswordRequest request)
        {
            ApplicationUser? user = null;

            if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userManager.FindByEmailAsync(request.Email);
            }
            else if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            }

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailNotConfirmed);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Reset Code for {Phone}/{Email}: {code}",
                request.PhoneNumber ?? request.Email, code);

            // ◀️ ممكن تبعت SMS هنا لو عايز
            SendResetPasswordEmail(user, code);

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ApplicationUser? user = null;

            if (!string.IsNullOrEmpty(request.Email))
                user = await _userManager.FindByEmailAsync(request.Email);
            else if (!string.IsNullOrEmpty(request.PhoneNumber))
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserErrors.InvalidCode);

            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status401Unauthorized));
        }
        private static string GenerateRefreshToken() =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private string GenerateMembershipId()
        {
            // Format: MEM-YYYY-XXXX
            return $"MEM-{DateTime.Now.Year}-{Random.Shared.Next(1000, 9999)}";
        }

        private string GenerateSequenceNumber()
        {
            return Random.Shared.Next(1000, 9999).ToString("D4");
        }

        private string GenerateDigitalAccessKey()
        {
            return Random.Shared.Next(1000, 9999).ToString("D4");
        }

        private void SendConfirmationEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin.FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Request.Headers.Referer.FirstOrDefault()
                        ?? "http://localhost:4200";

            var emailBody = EmailBodyBuilder.generateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    { "{{name}}", $"{user.FirstName} {user.LastName}" },
                    { "{{action_url}}", $"{origin}/Authentication/ConfirmEmail?UserId={user.Id}&Code={code}" }
                });

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "E-Club : Email Confirmation ✅", emailBody));
        }

        private void SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin.FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Request.Headers.Referer.FirstOrDefault()
                        ?? "http://localhost:4200";

            var emailBody = EmailBodyBuilder.generateEmailBody("ForgetPassword",
                new Dictionary<string, string>
                {
                    { "{{name}}", $"{user.FirstName} {user.LastName}" },
                    { "{{action_url}}", $"{origin}/Authentication/ResetPassword?Email={user.Email}&Code={code}" }
                });

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "E-Club : Reset Password ✅", emailBody));
        }

        private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermission(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userPermissions = await (from role in _context.Roles
                                         join claim in _context.RoleClaims
                                         on role.Id equals claim.RoleId
                                         where userRoles.Contains(role.Name!)
                                         select claim.ClaimValue!)
                                        .Distinct()
                                        .ToListAsync(cancellationToken);

            return (userRoles, userPermissions);
        }

        public async Task<Result> VerifyResetCodeAsync(VerifyResetCodeRequest request)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                var result = await _userManager.VerifyUserTokenAsync(user,
                    TokenOptions.DefaultProvider, "ResetPassword", code);

                return result
                    ? Result.Success()
                    : Result.Failure(UserErrors.InvalidCode);
            }
            catch
            {
                return Result.Failure(UserErrors.InvalidCode);
            }
        }
        public async Task<Result> ResendResetCodeAsync(ResendResetCodeRequest request)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("New Reset Code for {Phone}: {code}", request.PhoneNumber, code);

            // ◀️ هنا تبعت SMS تاني

            return Result.Success();
        }
        private string GenerateClubCode()
        {
            return $"CLUB-{Guid.NewGuid():N}".Substring(0, 14).ToUpper();
        }
    }
}