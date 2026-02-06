using eDoctor.Models;
using Microsoft.EntityFrameworkCore;

namespace eDoctor.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Introduction> Introductions { get; set; }
    public DbSet<Section> Sections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.LoginName)
            .IsUnique();

        modelBuilder.Entity<Introduction>()
            .HasIndex(i => new { i.DoctorId, i.SectionId })
            .IsUnique();

        modelBuilder.Entity<Doctor>()
            .HasIndex(d => d.LoginName)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasIndex(d => d.DepartmentName)
            .IsUnique();

        modelBuilder.Entity<Section>()
           .HasIndex(s => s.SectionTitle)
           .IsUnique();

        modelBuilder.Entity<Section>()
            .HasIndex(s => s.SectionOrder)
            .IsUnique();
    }
}
