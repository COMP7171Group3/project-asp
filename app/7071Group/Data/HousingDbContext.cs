using _7071Group.Models;
using Microsoft.EntityFrameworkCore;

namespace _7071Group.Data;

public class HousingDbContext : DbContext
{
    public HousingDbContext(DbContextOptions<HousingDbContext> options) : base(options) { }

    public DbSet<Asset> Assets
    {
        get; set;
    }
    public DbSet<Renter> Renters
    {
        get; set;
    }
    public DbSet<RentalHistory> RentalHistories
    {
        get; set;
    }
    public DbSet<DamageReport> DamageReports
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
}
