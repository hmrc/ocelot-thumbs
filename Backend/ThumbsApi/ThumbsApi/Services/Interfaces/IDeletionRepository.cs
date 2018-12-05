using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Services.Interfaces
{
    public interface IDeletionRepository
    {
        /// <summary>
        /// Adds a deletion to memory 
        /// </summary>
        /// <param name="deletion"></param>
        void Add(Deletion deletion);

        /// <summary>
        /// Removes a deletion from memory
        /// </summary>
        /// <param name="deletion"></param>
        void Delete(Deletion deletion);

        /// <summary>
        /// Gets a thumbs that meets the where criteria
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<Deletion> GetAsync(Expression<Func<Deletion, bool>> where);

        /// <summary>
        /// Gets all thumbs
        /// </summary>
        Task<List<Deletion>> GetManyAsync();

        /// <summary>
        /// Gets all thumbs that meet the where criteria
        /// </summary>
        Task<List<Deletion>> GetManyAsync(Expression<Func<Deletion, bool>> where);

        /// <summary>
        /// Gets a number thumbs that meet the where criteria ordered by ordered
        /// </summary>
        Task<List<Deletion>> GetManyAsync<T>(Expression<Func<Deletion, bool>> where, Expression<Func<Deletion, T>> orderBy, int take);

        /// <summary>
        /// Save all changes in memory
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Update a deletion in memory
        /// </summary>
        /// <param name="deletion"></param>
        void Update(Deletion deletion);
    }
}
