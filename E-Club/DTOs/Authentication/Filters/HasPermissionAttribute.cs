namespace E_Club.DTOs.Authentication.Filters
{
    public class HasPermissionAttribute(string permission):AuthorizeAttribute(permission)
    {

    }
}
