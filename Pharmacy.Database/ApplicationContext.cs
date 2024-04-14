
using Microsoft.EntityFrameworkCore;
using Pharmacy.Database.Conigurations;
using Pharmacy.Domain.Entity;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;

namespace Pharmacy.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) 
        {
        }
        public string DbPath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pharmacy.db");
        

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
                options.UseSqlite($"Data Source={DbPath}");
    }
}