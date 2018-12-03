using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThumbsApi.Contexts;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Services
{
    internal class ReportRepository : IReportRepository
    {

        private readonly Context _thumbContext;

        public ReportRepository(Context thumbContext)
        {
            _thumbContext = thumbContext;
        }

        public async Task<Report> GetAsync(DateTime startDate, DateTime endDate, string product)
        {
            var thumbs = await _thumbContext.Thumbs.Where(t =>
                                                           t.Date >= startDate &&
                                                           t.Date <= endDate &&
                                                           t.Product == product).ToListAsync();
            return new Report(
                product,
                thumbs.Where(t => t.Rating == true).Count(),
                thumbs.Where(t => t.Rating == false).Count()
                );
        }        
    }
}