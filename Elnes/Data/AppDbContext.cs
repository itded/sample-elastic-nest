using Elnes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elnes.Data;

public class AppDbContext : DbContext
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public AppDbContext()
    {
        // nothing
    }
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // nothing
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("Default");
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<Teacher> Teachers { get; set; }
    
    public DbSet<Subject> Subjects { get; set; }
    
    public DbSet<TeacherSubject> TeacherSubjects { get; set; }
}