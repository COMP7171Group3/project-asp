using _7071Group.Models;
using Microsoft.EntityFrameworkCore;

namespace _7071Group.Data;

public class CareDbContext : DbContext
{
    public CareDbContext(DbContextOptions<CareDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ServiceAssignment> ServiceAssignments { get; set; } = null!;
    public DbSet<ServiceRegistration> ServiceRegistrations { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ServiceAssignment>()
            .HasKey(sa => sa.AssignedID);

        builder.Entity<ServiceAssignment>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(sa => sa.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ServiceAssignment>()
            .HasOne<Service>()
            .WithMany()
            .HasForeignKey(sa => sa.ServiceID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ServiceRegistration>()
            .ToTable("Service_Registration")
            .HasKey(sr => new { sr.ClientID, sr.ServiceID });

        builder.Entity<ServiceRegistration>()
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(sr => sr.ClientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ServiceRegistration>()
            .HasOne<Service>()
            .WithMany()
            .HasForeignKey(sr => sr.ServiceID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invoice>()
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(i => i.ClientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
