
namespace E_Club.JWT
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _key;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        }

        public (string Token, int ExpiresIn) GenerateToken(
            ApplicationUser user,
            IEnumerable<string> roles,
            IEnumerable<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id), // simpler claim for easy access
                new Claim("fullName", $"{user.FirstName} {user.LastName}")
            };

            // Add roles claim
            if (roles?.Any() == true)
            {
                claims.Add(new Claim("roles", JsonSerializer.Serialize(roles),
                    JsonClaimValueTypes.JsonArray));

                // Also add individual role claims for easier filtering
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Add permissions claim
            if (permissions?.Any() == true)
            {
                claims.Add(new Claim("permissions", JsonSerializer.Serialize(permissions),
                    JsonClaimValueTypes.JsonArray));
            }

            var signingCredentials = new SigningCredentials(_key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
      issuer: _options.Issuer,
      audience: _options.Audience?.FirstOrDefault() ?? "https://e-club.runasp.net",  // ◀️ خذ أول قيمة
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
      signingCredentials: signingCredentials
  );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, _options.ExpiresInMinutes * 60); // return in seconds
        }

        public string? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = _key,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = _options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _options.Audience?.FirstOrDefault() ?? "https://e-club.runasp.net"  ,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // مهم جداً للـ expiration
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Try to get subject claim first, then nameidentifier
                var userId = jwtToken.Claims.FirstOrDefault(x =>
                    x.Type == JwtRegisteredClaimNames.Sub)?.Value
                    ?? jwtToken.Claims.FirstOrDefault(x =>
                        x.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
            catch (SecurityTokenExpiredException)
            {
                // Token expired - ممكن نعمل handling مخصوص
                return null;
            }
            catch
            {
                return null;
            }
        }

        public int? GetExpirationFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.ValidTo.Second;
            }
            catch
            {
                return null;
            }
        }

        // Method to refresh token
        public string? RefreshToken(string expiredToken)
        {
            // Logic to refresh token if needed
            throw new NotImplementedException();
        }
    }
}