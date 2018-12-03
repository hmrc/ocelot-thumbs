using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Contexts
{
    public class Context:DbContext
    {
        private ILogger<Context> _logger;

        public Context(DbContextOptions<Context> options, ILogger<Context> logger)
            : base(options)
        {
            _logger = logger;
            Database.EnsureCreated();
        }

        public DbSet<Thumb> Thumbs { get; set; }
    }
}
