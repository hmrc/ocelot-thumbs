using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("{product}")]
        public async Task<IActionResult> Get(string product, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            
            var report = await _reportRepository.GetAsync(startDate, endDate, product);

            //todo get grouping from api and loop through all children;
            //foreach (child in grouping.Children)
            //{
            //    report.Children.Add(_reportRepository.GetAsync(startDate, endDate, child.Product);)
            //}

            return Ok(report);
        }
    }
}
