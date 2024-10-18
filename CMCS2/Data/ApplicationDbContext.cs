using CMCS2.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configures decimal precision for the Claim entity
            modelBuilder.Entity<Claim>(entity =>
            {
                entity.Property(e => e.HourlyRate).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Claim>().ToTable("Claims");
        }

        // Add DbSet properties for each entity model you have
        public DbSet<Claim> Claims { get; set; } // Example entity model
        public DbSet<Lecturer> Lecturers { get; set; } // Example entity model
        public DbSet<Coordinator> Coordinators { get; set; } // Example entity model
    }
}
