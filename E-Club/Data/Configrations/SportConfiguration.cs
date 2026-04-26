namespace E_Club.Data.Configurations;

public class SportConfiguration : IEntityTypeConfiguration<Sport>
{
    public void Configure(EntityTypeBuilder<Sport> builder)
    {
        builder.ToTable("Sports");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Icon).HasMaxLength(50);

        builder.HasData(
            new Sport { Id = 1, Name = "Football", Icon = "sports_soccer", IsActive = true, DisplayOrder = 1, CreatedOn = DateTime.UtcNow, CreatedById =null },
            new Sport { Id = 2, Name = "Basketball", Icon = "sports_basketball", IsActive = true, DisplayOrder = 2, CreatedOn = DateTime.UtcNow, CreatedById = null },
            new Sport { Id = 3, Name = "Tennis", Icon = "sports_tennis", IsActive = true, DisplayOrder = 3, CreatedOn = DateTime.UtcNow, CreatedById = null }
        );
    }
}
