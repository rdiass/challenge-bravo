using Microsoft.EntityFrameworkCore;
using Challenge.Bravo.Api.Models;
using MongoDB.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Bravo.Api.Data
{
    public class CurrencyDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>().ToCollection("Currencies");
        }
    }
}
