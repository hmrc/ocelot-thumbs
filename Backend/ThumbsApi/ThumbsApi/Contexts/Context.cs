using Microsoft.EntityFrameworkCore;
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

        internal DbSet<Deletion> Deletions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thumb>()
                        .Property(p => p.Pid)
                        .IsFixedLength()
                        .IsUnicode(false);

            modelBuilder.Entity<Deletion>()
                        .Property(p => p.Pid)
                        .IsFixedLength()
                        .IsUnicode(false);

            modelBuilder.Entity<Deletion>()
                        .Property(p => p.DeletedBy)
                        .IsFixedLength()
                        .IsUnicode(false);
        }
    }
}
