namespace E_Club.Consts;

public static class RegexPatterns
{
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
}