﻿using BazaSmyczy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BazaSmyczy.Core.Stores.Data
{
    public class LeashDbContext : DbContext
    {
        public LeashDbContext(DbContextOptions<LeashDbContext> options) : base(options)
        {
        }

        public DbSet<Leash> Leashes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Leash>().ToTable("Leash");
        }
    }
}
