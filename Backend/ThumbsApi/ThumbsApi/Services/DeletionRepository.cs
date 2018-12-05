using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThumbsApi.Contexts;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Services
{
    public class DeletionRepository : IDeletionRepository
    {
        private readonly Context _context;

        public DeletionRepository(Context context)
        {
            _context = context;
        }

        public void Add(Deletion items)
        {
            _context.Deletions
                         .Add(items);
        }

        public void Delete(Deletion deletion)
        {
            _context.Deletions
                         .Remove(deletion);
        }

        public Task<Deletion> GetAsync(Expression<Func<Deletion, bool>> where)
        {
            return _context.Deletions
                                .Where(where)
                                .FirstOrDefaultAsync();
        }

        public Task<List<Deletion>> GetManyAsync()
        {
            return _context.Deletions
                              .ToListAsync();

        }

        public Task<List<Deletion>> GetManyAsync(Expression<Func<Deletion, bool>> where)
        {
            return _context.Deletions
                                .Where(where)
                                .ToListAsync();
        }

        public Task<List<Deletion>> GetManyAsync<T>(Expression<Func<Deletion, bool>> where, Expression<Func<Deletion, T>> orderBy, int take)
        {
            return _context.Deletions
                                .Where(where)
                                .OrderBy(orderBy)
                                .Take(take)
                                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Deletion deletion)
        {
            //not needed in this implementation
        }
    }
}
