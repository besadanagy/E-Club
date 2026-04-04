
namespace E_Club.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        // Event Featured (بيانات افتراضية)
        builder.HasData(new Event
        {
            Id = 1,
            Title = "Annual Football Championship",
            Description = "Join us for the biggest football event of the year!",
            Location = "Main Arena, Smart Club",
            StartDate = new DateTime(2023, 10, 24, 9, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2023, 10, 26, 18, 0, 0, DateTimeKind.Utc),
            IsFeatured = true,
            MaxParticipants = 100,
            CurrentParticipants = 45,
            Status = EventStatus.Upcoming,
            CreatedOn = DateTime.UtcNow,
            // CreatedById = "System"  // ◀️ علق السطر ده
            CreatedById = null          // ◀️ أو استخدم null
        });
    }
}