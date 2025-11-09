using CSaver.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CSaver.DB;

public class ApplicationDbContext: DbContext
{
    public DbSet<Delivery> Deliveries { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=MyDB;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>()
            .HasIndex(d => new { d.tpep_pickup_datetime, d.tpep_dropoff_datetime, d.passenger_count })
            .IsUnique(true);

        base.OnModelCreating(modelBuilder);
    }
}