using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace DataAccess
{
    public class PollDbContext : DbContext
    {
        public PollDbContext(DbContextOptions<PollDbContext> options)
            : base(options) { }

        public DbSet<Poll> Polls => Set<Poll>();
        public DbSet<User> Users => Set<User>();
        public DbSet<PollVote> PollVotes => Set<PollVote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // created an initial user for testing purposes
            modelBuilder.Entity<User>().HasData(new User 
            { 
                Id = 1, 
                Username = "admin", 
                Password = "admin" 
            });
        }
    }
}
