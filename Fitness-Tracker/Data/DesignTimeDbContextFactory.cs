using Fitness_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AlternativeDbContext>
{
    public AlternativeDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AlternativeDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("daysDb"));

        return new AlternativeDbContext(optionsBuilder.Options);
    }
}
