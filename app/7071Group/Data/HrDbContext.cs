using _7071Group.Models;
using Microsoft.EntityFrameworkCore;

namespace _7071Group.Data;

public class HrDbContext : DbContext
{
    public HrDbContext(DbContextOptions<HrDbContext> options) : base(options) { }

    public DbSet<Employee> Employees
    {
        get; set;
    }
    public DbSet<Shift> Shifts
    {
        get; set;
    }
    public DbSet<Payroll> Payrolls
    {
        get; set;
    }
    public DbSet<Attendance> Attendances
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Employee to Manager self-reference
        builder.Entity<Employee>()
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(e => e.ReportsTo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
