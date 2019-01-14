using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
//#if DEBUG
//            uri = new Uri("https://localhost:44310/ProductGroupsdata");
//#else     
            uri = new Uri("https://internal-apps.guidance.prod.dop.corp.hmrc.gov.uk/ProductGrouping/ProductGroupsData");
//#endif
        }

        public async Task<ProductGroup> GetAsync(string groupName)
        {
            if (DateTime.Now >= lastUpdated.AddMinutes(10) && !isUpdating)
            {
                await UpdateGroups();
            }

            return groups?.Where(g => g.ProductReference.ToUpper() == groupName.ToUpper()).FirstOrDefault();
        }

        private async Task UpdateGroups()
        {
            try
            {
                isUpdating = true;
                await GetGroups();
                lastUpdated = DateTime.Now;
            }
            catch (Exception ex)
            {
                logger.LogCritical("504", ex.Message, ex);
            }
            finally
            {
                isUpdating = false;
            }
        }

        private async Task GetGroups()
        {
            using (var client = new HttpClient(new HttpClientHandler()
            {
                PreAuthenticate = true,
                UseDefaultCredentials = true
            })
            {
                Timeout = new TimeSpan(0,0,5),
            })
            {
                var report = new HttpRequestMessage(HttpMethod.Get, uri);
                var reportResult = await client.SendAsync(report);
                reportResult.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<ICollection<ProductGroup>>
                (
                    await reportResult.Content.ReadAsStringAsync()
                );
                groups = result.Expand(g => g.Children);
            }
        }
    }
}
