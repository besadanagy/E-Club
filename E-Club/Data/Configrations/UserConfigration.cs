namespace E_Club.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var admin = new ApplicationUser
            {
                Id = "22222222-2222-2222-2222-222222222222",
                UserName = "admin@eclub.com",
                NormalizedUserName = "ADMIN@ECLUB.COM",
                Email = "admin@eclub.com",
                NormalizedEmail = "ADMIN@ECLUB.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User"
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123456");
            builder.HasData(admin);
        }
    }
}