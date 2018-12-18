using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Services
{
    public class GroupingRepository : IGroupingRepository
    {
        
        private Uri uri;

        private static IEnumerable<ProductGroup> groups = new List<ProductGroup>();
        private static DateTime lastUpdated;
        private static bool isUpdating = false;
        private ILogger<GroupingRepository> logger;

        public GroupingRepository(ILogger<GroupingRepository> logger)
        {
            this.logger = logger;

            //todo pull from environment
#if DEBUG
            uri = new Uri("https://localhost:44310/ProductGroupsdata");
#else     
            uri = new Uri("https://apps.guidance.prod.dop.corp.hmrc.gov.uk/ProductGrouping/ProductGroupsData");
#endif
        }

        public async Task<ProductGroup> GetAsync(string groupName)
        {
            if (DateTime.Now >= lastUpdated.AddMinutes(10) && !isUpdating)
            {
                await UpdateGroups();
            }

            return groups.Where(g => g.ProductReference.ToUpper() == groupName.ToUpper()).FirstOrDefault();
        }

        private async Task UpdateGroups()
        {
            await GetGroups();
            lastUpdated = DateTime.Now;
        }

        private async Task GetGroups()
        {
            try
            {
                isUpdating = true;

                using (var client = new WebClient()
                {
                    UseDefaultCredentials = true
                })
                {
                    var json = await client.DownloadStringTaskAsync(uri);
                    var result = JsonConvert.DeserializeObject<ICollection<ProductGroup>>(json);
                    groups = result.Expand(g => g.Children);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("500", ex.Message, ex);
            }
            finally
            {
                isUpdating = false;
            }
        }
    }
}
