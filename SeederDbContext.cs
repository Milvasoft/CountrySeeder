using CountrySeeder.Entities;
using Microsoft.EntityFrameworkCore;
using Milvasoft.Core.Helpers.GeoLocation.Models;
using System.Text.Json;

namespace CountrySeeder;
public class AppDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TableCheckResult>().HasNoKey();

        modelBuilder.Entity<Country>()
            .Property(c => c.GeoPoint)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions)null)
            );

        modelBuilder.Entity<Country>()
            .Property(c => c.Translations)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
            );

        modelBuilder.Entity<Country>()
            .Property(c => c.Timezones)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
            );

        modelBuilder.Entity<State>()
            .Property(s => s.GeoPoint)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions)null)
            );

        modelBuilder.Entity<City>()
            .Property(c => c.GeoPoint)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<GeoPoint>(v, (JsonSerializerOptions)null)
            );
    }
}