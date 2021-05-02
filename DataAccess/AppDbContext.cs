using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<Shopper> Shoppers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine); // To easily see SQL logs on console.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // to make table and column names start with lowercase character.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(
                    $"{entityType.GetTableName().Substring(0, 1).ToLowerInvariant()}{entityType.GetTableName()[1..]}");

                foreach (var property in entityType.GetProperties())
                    property.SetColumnName(
                        $"{property.GetColumnName().Substring(0, 1).ToLowerInvariant()}{property.GetColumnName()[1..]}");
            }
        }

    }
}
