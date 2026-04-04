
namespace E_Club.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.Icon)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Endpoint)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.ImageUrl)
            .HasMaxLength(500);

        builder.Property(s => s.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        // Seed Data للخدمات الأساسية
        builder.HasData(
            new Service
            {
                Id = 1,
                Name = "Book a Field",
                Description = "Reserve your favorite football field",
                Icon = "sports_soccer",
                Endpoint = "/book-field",
                IsActive = true,
                DisplayOrder = 1,
                Type = ServiceType.Booking,
                CreatedOn = DateTime.UtcNow,
                CreatedById = "22222222-2222-2222-2222-222222222222"
            },
            new Service
            {
                Id = 2,
                Name = "Join Tournament",
                Description = "Participate in upcoming tournaments",
                Icon = "emoji_events",
                Endpoint = "/join-tournament",
                IsActive = true,
                DisplayOrder = 2,
                Type = ServiceType.Tournament,
                CreatedOn = DateTime.UtcNow,
                CreatedById = "22222222-2222-2222-2222-222222222222"
            },
            new Service
            {
                Id = 3,
                Name = "Personal Coaching",
                Description = "Get one-on-one coaching",
                Icon = "sports",
                Endpoint = "/personal-coaching",
                IsActive = true,
                DisplayOrder = 3,
                Type = ServiceType.Coaching,
                CreatedOn = DateTime.UtcNow,
                CreatedById = "22222222-2222-2222-2222-222222222222"
            }
        );
    }
}