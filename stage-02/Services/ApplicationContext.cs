using Microsoft.EntityFrameworkCore;

using hngx_duo.Models;

namespace hngx_duo.Services;

public class ApplicationContext : DbContext
{
    public DbSet<Person> People { get; set; } = null!;
    public string DbPath { get; }

    public ApplicationContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder, Environment.SpecialFolderOption.Create);

        DbPath = Path.Join(path, "people.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .ToTable(t => t.HasCheckConstraint("Ck_Age", "[Age] > 0"));
    }
}

