namespace E_Club.Data.Configurations;

public class SportClassConfiguration : IEntityTypeConfiguration<SportClass>
{
    public void Configure(EntityTypeBuilder<SportClass> builder)
    {
        builder.ToTable("SportClasses");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Location).IsRequired().HasMaxLength(200);

        builder.HasOne(c => c.Sport)
            .WithMany(s => s.Classes)
            .HasForeignKey(c => c.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Price)
    .HasPrecision(18, 2);  // 18 digits total, 2 after decimal point
    }
}
