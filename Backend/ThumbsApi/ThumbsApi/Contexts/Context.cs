﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Contexts
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            if (!Database.EnsureCreated())
            {
                Database.Migrate();
            }
        }

        public DbSet<Thumb> Thumbs { get; set; }
    }
}
