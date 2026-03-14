using E_Club.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Org.BouncyCastle.Utilities;
using System.Reflection;
using System.Security.Claims;

namespace E_Club.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSets
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        //public DbSet<Branch> Branches { get; set; }           // ◀️ مثال
        //public DbSet<Sport> Sports { get; set; }               // ◀️ مثال

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تطبيق الـ Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // منع Cascade Delete
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Configurations للـ Entities
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasMany(u => u.RefreshTokens)
                    .WithOne(rt => rt.User)                    // ◀️ مهم: .WithOne(rt => rt.User)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired();
                entity.Property(rt => rt.ExpiresOn).IsRequired();

                entity.HasIndex(rt => rt.Token).IsUnique();
                entity.HasIndex(rt => rt.UserId);

                // ◀️ علاقة الـ RefreshToken مع User
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>().ToList();

            foreach (var entityEntry in entries)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId ?? "System";
                    entityEntry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}