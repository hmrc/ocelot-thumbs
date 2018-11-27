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
    public class ThumbsRepository : IThumbsRepository
    {
        private readonly Context _thumbContext;

        public ThumbsRepository(Context thumbContext)
        {
            _thumbContext = thumbContext;
        }

        public void Add(Thumb items)
        {
            _thumbContext.Thumbs
                         .Add(items);
        }

        public void Delete(Thumb thumb)
        {
            _thumbContext.Thumbs
                         .Remove(thumb);
        }

        public Task<Thumb> GetAsync(Expression<Func<Thumb, bool>> where)
        {
            return _thumbContext.Thumbs
                                .Where(where)
                                .FirstOrDefaultAsync();
        }

        public Task<List<Thumb>> GetManyAsync()
        {
            return _thumbContext.Thumbs
                              .ToListAsync();

        }

        public Task<List<Thumb>> GetManyAsync(Expression<Func<Thumb, bool>> where)
        {
            return _thumbContext.Thumbs
                                .Where(where)
                                .ToListAsync();
        }

        public Task<List<Thumb>> GetManyAsync<T>(Expression<Func<Thumb, bool>> where, Expression<Func<Thumb, T>> orderBy, int take)
        {
            return _thumbContext.Thumbs
                                .Where(where)
                                .OrderBy(orderBy)
                                .Take(take)
                                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _thumbContext.SaveChangesAsync() > 0;
        }

        public void Update(Thumb thumb)
        {
            //not needed in this implementation
        }
    }
}
