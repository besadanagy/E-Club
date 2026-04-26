namespace E_Club.Data.Configurations;

public class ClassBookingConfiguration : IEntityTypeConfiguration<ClassBooking>
{
    public void Configure(EntityTypeBuilder<ClassBooking> builder)
    {
        builder.ToTable("ClassBookings");
        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.SportClass)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.SportClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => new { b.SportClassId, b.UserId }).IsUnique();
    }
}
