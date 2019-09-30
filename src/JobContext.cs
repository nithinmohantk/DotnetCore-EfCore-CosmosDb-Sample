using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore3_cosmos
{
    public class JobContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Resource> Resources { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "EFCoreDemo2"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().OwnsOne(j => j.Address);

            modelBuilder.Entity<Job>().OwnsMany(j => j.Contacts);

            modelBuilder.Entity<Job>().HasOne(j => j.AssignedResource);
        }
    }
}
