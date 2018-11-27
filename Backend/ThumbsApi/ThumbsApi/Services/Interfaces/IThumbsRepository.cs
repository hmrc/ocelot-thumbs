using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Services.Interfaces
{
   public interface IThumbsRepository
    {
        /// <summary>
        /// Adds a thumb to memory 
        /// </summary>
        /// <param name="thumb"></param>
        void Add(Thumb thumb);

        /// <summary>
        /// Removes a thumb from memory
        /// </summary>
        /// <param name="thumb"></param>
        void Delete(Thumb thumb);

        /// <summary>
        /// Gets a thumbs that meets the where criteria
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<Thumb> GetAsync(Expression<Func<Thumb, bool>> where);

        /// <summary>
        /// Gets all thumbs
        /// </summary>
        Task<List<Thumb>> GetManyAsync();

        /// <summary>
        /// Gets all thumbs that meet the where criteria
        /// </summary>
        Task<List<Thumb>> GetManyAsync(Expression<Func<Thumb, bool>> where);

        /// <summary>
        /// Gets a number thumbs that meet the where criteria ordered by ordered
        /// </summary>
        Task<List<Thumb>> GetManyAsync<T>(Expression<Func<Thumb, bool>> where, Expression<Func<Thumb, T>> orderBy, int take);

        /// <summary>
        /// Save all changes in memory
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Update a thumb in memory
        /// </summary>
        /// <param name="thumb"></param>
        void Update(Thumb thumb);
    }
}
