using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThumbsApi.Models;

namespace ThumbsApi.Contexts
{
    internal class Context:DbContext
    {
        internal Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        internal DbSet<Thumb> Thumbs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thumb>()
                        .Property(p => p.Pid)
                        .IsFixedLength()
                        .IsUnicode(false);
        }
    }
}
