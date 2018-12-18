using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThumbsApi.Contexts;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Services
{
    public class ReportRepository : IReportRepository
    {

        private readonly Context _thumbContext;

        public ReportRepository(Context thumbContext)
        {
            _thumbContext = thumbContext;
        }

        public async Task<Report> GetAsync(DateTime startDate, DateTime endDate, ProductGroup product)
        {
            var thumbs = await _thumbContext.Thumbs.Where(t =>
                                                          t.Date >= startDate &&
                                                          t.Date <= endDate &&
                                                          GetProductNames(product).Contains(t.Product))
                                                   .ToListAsync();

            return Mapper(product, thumbs);

        }        

        private IEnumerable<string> GetProductNames(ProductGroup product)
        {
            var names = new List<string> { product.ProductReference };
            names.AddRange(product.Children.Expand(c => c.Children).Select(e => e.ProductReference));
            return names;
        }

        private Report Mapper(ProductGroup product, IEnumerable<Thumb> thumbs)
        {
            var report = new Report
            (
                product.ProductReference,
                thumbs.Where(t => t.Product == product.ProductReference).Count(t => t.Rating),
                thumbs.Where(t => t.Product == product.ProductReference).Count(t => !t.Rating)
            );

            if (product.Children?.Any() == true)
            {
                Parallel.ForEach(product.Children, c => report.Children.Add(Mapper(c, thumbs))); 
            }

            return report;
        }
    }
}