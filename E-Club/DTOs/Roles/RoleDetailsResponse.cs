namespace E_Club.DTOs.Roles;

public record RoleDetailsResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions,
    IEnumerable<UserInRoleResponse> Users  // ◀️ UserInRoleResponse
);