namespace E_Club.Errors
{
    public static class RoleErrors // خليها static class
    {
        // Role specific errors
        public static readonly Error InvalidCredentials =
           new("Role.InvalidCredentials", "Invalid email / password", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidPermission =
            new("Role.InvalidPermission", "You don't have permission to perform this action",
                StatusCodes.Status403Forbidden); // 403 Forbidden أنسب من 400

        public static readonly Error DuplicatedRole = // تصحيح إملائي Dublicated → Duplicated
            new("Role.DuplicatedRole", "Role already exists", StatusCodes.Status409Conflict);

        public static readonly Error RoleNotFound =
            new("Role.NotFound", "Role not found", StatusCodes.Status404NotFound);

        public static readonly Error RoleHasUsers =
            new("Role.HasUsers", "Cannot delete role because it has assigned users",
                StatusCodes.Status400BadRequest);

        public static readonly Error CannotDeleteDefaultRole =
            new("Role.CannotDeleteDefault", "Cannot delete default system role",
                StatusCodes.Status400BadRequest);

        // User related (يفضل تكون في UserErrors)
        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

        public static readonly Error EmailIsConfirmed =
            new("User.EmailIsConfirmed", "Email already confirmed", StatusCodes.Status409Conflict);
    }
}