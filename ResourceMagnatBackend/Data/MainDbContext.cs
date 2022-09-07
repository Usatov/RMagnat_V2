using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Models;

namespace ResourceMagnat.Data;

public class MainDbContext: DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Добавляем уникальный индекс
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Uid)
            .IsUnique();
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Building> Buildings { get; set; }

    public DbSet<BuildingType> BuildingTypes { get; set; }

    public DbSet<Session> Sessions { get; set; }
}