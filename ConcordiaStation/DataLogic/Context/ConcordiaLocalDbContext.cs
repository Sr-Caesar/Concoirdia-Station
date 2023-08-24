using ConcordiaStation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaStation.Data.Context
{
    public class ConcordiaLocalDbContext : DbContext
    {
        public DbSet<Phase> Phases { get; set; } = null!;
        public DbSet<Scientist> Scientists { get; set; } = null!;
        public DbSet<Experiment> Experiments { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Credential> Credentials { get; set; } = null!;

        public ConcordiaLocalDbContext(DbContextOptions<ConcordiaLocalDbContext> options) : base(options)
        {
        }
    }
}