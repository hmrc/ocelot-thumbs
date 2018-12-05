using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThumbsApi.Models;

namespace ThumbsApi.Contexts
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Thumb> Thumbs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thumb>()
                        .Property(p => p.Pid)
                        .IsFixedLength()
                        .IsUnicode(false);
        }
    }
}
