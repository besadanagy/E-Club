public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var roles = new List<ApplicationRole>
        {
            new ApplicationRole
            {
                Id = "403f3843-7660-40ec-9ddf-eb2066ab287f",
                Name = "Admin",
                NormalizedName = "ADMIN",
                IsDefault = true,
                IsDeleted = false,
                ConcurrencyStamp = null
            },
            new ApplicationRole
            {
                Id = "81184fbe-d733-4e41-9b39-1a6e5e97f6b6",
                Name = "Member",
                NormalizedName = "MEMBER",
                IsDefault = true,
                IsDeleted = false,
                ConcurrencyStamp = null
            },
            new ApplicationRole
            {
                Id = "1800d77e-6d31-4faf-87e8-db0609459f17",
                Name = "Coach",
                NormalizedName = "COACH",
                IsDefault = false,
                IsDeleted = false,
                ConcurrencyStamp = null
            }
        };

        builder.HasData(roles);
    }
}