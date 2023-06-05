using Microsoft.EntityFrameworkCore;
using Fitness_Tracker.Models;

namespace Fitness_Tracker.Data
{
    public class AlternativeDbContext : DbContext
    {
        public AlternativeDbContext(DbContextOptions<AlternativeDbContext> options) : base(options) 
        { 
        }

        public DbSet<Day> Days { get; set; }
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Stat> Stats { get; set; }
    }
}
