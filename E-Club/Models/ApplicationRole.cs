namespace E_Club.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }

        // Constructor
        public ApplicationRole() : base() { }

        public ApplicationRole(string roleName) : base(roleName) { }

        public ApplicationRole(string roleName, bool isDefault = false) : base(roleName)
        {
            IsDefault = isDefault;
        }
    }
}