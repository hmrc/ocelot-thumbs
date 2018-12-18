using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Services.Interfaces
{
    public interface IGroupingRepository
    {
        Task<ProductGroup> GetAsync(string groupName);
    }
}
