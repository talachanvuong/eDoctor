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
    public DbSet<Service> Services { get; set; }
    public DbSet<DetailPrescription> DetailPrescriptions { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<DetailInvoice> DetailInvoices { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Schedule> Schedules { get; set; }

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

        modelBuilder.Entity<Service>()
            .HasIndex(s => s.ServiceName)
            .IsUnique();

        modelBuilder.Entity<DetailPrescription>()
           .HasIndex(d => d.DrugName)
           .IsUnique();

        modelBuilder.Entity<DetailInvoice>()
           .HasIndex(d => d.ServiceName)
           .IsUnique();

        modelBuilder.Entity<Payment>()
           .HasIndex(p => p.GatewayOrderId)
           .IsUnique();

        modelBuilder.Entity<Schedule>()
           .HasIndex(s => s.Room)
           .IsUnique();

        modelBuilder.Entity<Schedule>()
            .ToTable(t => t.HasCheckConstraint(
                "CK_Schedule_Time",
                "EndTime > StartTime"
            ));

        modelBuilder.Entity<Schedule>()
           .HasIndex(s => new { s.DoctorId, s.UserId })
           .IsUnique();

        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetailInvoice>()
            .Property(d => d.Price)
            .HasPrecision(18, 2);
    }
}
