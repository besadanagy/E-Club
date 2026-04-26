namespace E_Club.Data.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("Banners");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Subtitle).IsRequired().HasMaxLength(500);
        builder.Property(b => b.ImageUrl).IsRequired().HasMaxLength(500);
        builder.Property(b => b.ActionUrl).HasMaxLength(200);

        builder.HasData(
            new Banner
            {
                Id = 1,
                Title = "New Tournament",
                Subtitle = "Registrations open",
                ImageUrl = "/images/banners/tournament.jpg",
                Type = BannerType.Tournament,
                IsActive = true,
                DisplayOrder = 1,
                CreatedOn = DateTime.UtcNow,
                CreatedById = null
            }
        );
    }
}
