using DripChip.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Models;

public sealed class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<AnimalType> AnimalTypes { get; set; } = null!;

    // public DbSet<VisitedLocation> VisitedLocations { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}