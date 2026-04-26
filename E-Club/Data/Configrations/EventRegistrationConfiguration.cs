namespace E_Club.Data.Configurations;

public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder.ToTable("EventRegistrations");
        builder.HasKey(er => er.Id);

        builder.HasOne(er => er.Event)
            .WithMany()
            .HasForeignKey(er => er.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(er => er.User)
            .WithMany()
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(er => new { er.EventId, er.UserId }).IsUnique();
    }
}
