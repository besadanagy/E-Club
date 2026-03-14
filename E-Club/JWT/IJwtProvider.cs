namespace E_Club.JWT
{
    public interface IJwtProvider
    {
        (string Token, int ExpiresIn) GenerateToken(
            ApplicationUser user,
            IEnumerable<string> roles,
            IEnumerable<string> permissions);

        string? ValidateToken(string token);

        // ممكن نضيف method إضافية
        int? GetExpirationFromToken(string token);
    }
}