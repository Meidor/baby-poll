using Microsoft.EntityFrameworkCore;
using BabyPoll.Models;
using System.Diagnostics.CodeAnalysis;
using System;

namespace BabyPoll
{
    public class BabyPollContext : DbContext
    {
        public BabyPollContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>().Property(e => e.Guess).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            modelBuilder.Entity<Poll>().Property(e => e.DueDate).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}