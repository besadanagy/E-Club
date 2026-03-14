namespace E_Club.Data.Configurations
{
    public class RoleClaimsConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            // جلب كل الصلاحيات
            var permissions = Permissions.GetAllPermissions();

            // Admin Role Id (ثابتة من الـ Seed)
            var adminRoleId = "403f3843-7660-40ec-9ddf-eb2066ab287f";

            var adminClaims = new List<IdentityRoleClaim<string>>();
            int id = 1;

            foreach (var permission in permissions)
            {
                adminClaims.Add(new IdentityRoleClaim<string>
                {
                    Id = id++,
                    RoleId = adminRoleId,
                    ClaimType = Permissions.Type,
                    ClaimValue = permission
                });
            }

            builder.HasData(adminClaims);
        }
    }
}