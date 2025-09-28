using _7071Group.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _7071Group.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<ServiceAssignment> ServiceAssignments { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ServiceRegistration> ServiceRegistrations { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Renter> Renters { get; set; }
    public DbSet<RentalHistory> RentalHistories { get; set; }
    public DbSet<DamageReport> DamageReports { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Employee to Manager self-reference
        ConfigureEmployeeManagerRelationship(builder);

        // Composite Keys & Foreign Key relationships
        ConfigureCompositeKeyAndForeignKeys(builder);

        // Indexes for performance
        ConfigureIndexes(builder);

        // Length constraints
        ConfigureMaxLengths(builder);
    }

    private void ConfigureEmployeeManagerRelationship(ModelBuilder builder)
    {
        builder.Entity<Employee>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(e => e.ReportsTo) // FK to manager's EmployeeID
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureCompositeKeyAndForeignKeys(ModelBuilder builder)
    {
        builder.Entity<ServiceAssignment>()
            .ToTable("Service_Assignment")
            .HasKey(sa => new { sa.EmployeeID, sa.ServiceID });
        builder.Entity<ServiceAssignment>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(sa => sa.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Shift>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(s => s.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payroll>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(p => p.EmployeeID)
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

        builder.Entity<RentalHistory>()
            .ToTable("Rental_History")
            .HasKey(rh => new { rh.AssetID, rh.RenterID });
        builder.Entity<RentalHistory>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(rh => rh.AssetID)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<RentalHistory>()
            .HasOne<Renter>()
            .WithMany()
            .HasForeignKey(rh => rh.RenterID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DamageReport>()
            .ToTable("Damage_Report")
            .HasKey(dr => dr.ReportID);

        builder.Entity<DamageReport>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(dr => dr.AssetID)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureIndexes(ModelBuilder builder)
    {
        builder.Entity<ServiceAssignment>()
            .HasIndex(sa => sa.EmployeeID);
        builder.Entity<ServiceAssignment>()
            .HasIndex(sa => sa.ServiceID);
        builder.Entity<ServiceAssignment>()
            .HasIndex(sa => new { sa.EmployeeID, sa.ServiceID });

        builder.Entity<Shift>()
            .HasIndex(s => s.EmployeeID);

        builder.Entity<Payroll>()
            .HasIndex(p => p.EmployeeID);

        builder.Entity<ServiceRegistration>()
            .HasIndex(sr => sr.ClientID);
        builder.Entity<ServiceRegistration>()
            .HasIndex(sr => sr.ServiceID);
        builder.Entity<ServiceRegistration>()
            .HasIndex(sr => new { sr.ClientID, sr.ServiceID });

        builder.Entity<Invoice>()
            .HasIndex(i => i.ClientID);

        builder.Entity<RentalHistory>()
            .HasIndex(rh => rh.AssetID);
        builder.Entity<RentalHistory>()
            .HasIndex(rh => rh.RenterID);
        builder.Entity<RentalHistory>()
            .HasIndex(rh => new { rh.AssetID, rh.RenterID });

        builder.Entity<DamageReport>()
            .HasIndex(dr => dr.AssetID);
    }

    private void ConfigureMaxLengths(ModelBuilder builder)
    {
        builder.Entity<Client>()
            .Property(c => c.Name)
            .HasMaxLength(100);

        builder.Entity<Employee>()
            .Property(e => e.Name)
            .HasMaxLength(100);

        builder.Entity<Employee>()
            .Property(e => e.JobTitle)
            .HasMaxLength(50);

        builder.Entity<Renter>()
            .Property(r => r.Name)
            .HasMaxLength(100);

        builder.Entity<Service>()
            .Property(s => s.ServiceName)
            .HasMaxLength(100);

        builder.Entity<DamageReport>()
            .Property(dr => dr.Description)
            .HasMaxLength(500);
    }
}
