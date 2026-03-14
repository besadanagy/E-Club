namespace E_Club.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
           new("User.InvalidCredentials", "Invalid email/password or membership credentials", StatusCodes.Status401Unauthorized);

        public static readonly Error DisabledUser =
           new("User.Disabled", "Your account has been disabled. Please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error LockedUser =
           new("User.Locked", "Your account has been locked. Please try again later", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedEmail =
            new("User.DuplicatedEmail", "Email is already registered", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
            new("User.EmailNotConfirmed", "Please confirm your email address", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid or expired code", StatusCodes.Status400BadRequest);

        public static readonly Error EmailIsConfirmed =
            new("User.EmailAlreadyConfirmed", "Email is already confirmed", StatusCodes.Status409Conflict);

        public static readonly Error UserNotFound =
            new("User.NotFound", "User not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidRole =
            new("User.InvalidRole", "Invalid role specified", StatusCodes.Status400BadRequest);
    }
}