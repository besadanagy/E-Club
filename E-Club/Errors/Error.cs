namespace E_Club.Errors;

public record Error(string Code, string Description, int? StatusCode = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);

    // Errors شوية ثابتة مفيدين
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.", 400);
    public static readonly Error Unexpected = new("Error.Unexpected", "An unexpected error occurred.", 500);
}