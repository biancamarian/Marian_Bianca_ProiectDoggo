using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectDoggo.Models;

namespace ProiectDoggo.Data
{
    public class ShopContext:DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) 
        { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Doggo> Doggoes { get; set; }
        public DbSet<Kennel> Kennels { get; set; }
        public DbSet<KennelDoggo> KennelDoggoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Doggo>().ToTable("Doggo");
            modelBuilder.Entity<Kennel>().ToTable("Kennel");
            modelBuilder.Entity<KennelDoggo>().ToTable("KennelDoggo");

            modelBuilder.Entity<KennelDoggo>()
                .HasKey(c => new { c.DoggoID, c.KennelID });//cheie   primara compusa
        }
    }
}
