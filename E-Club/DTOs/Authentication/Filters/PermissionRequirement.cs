namespace E_Club.DTOs.Authentication.Filters
{
    public class PermissionRequirement(string permission): IAuthorizationRequirement
    {
        public string Permission { get; }=permission;
    }
}
