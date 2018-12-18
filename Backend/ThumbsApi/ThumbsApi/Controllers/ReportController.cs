using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Controllers
{

    /// <summary>
    /// Controller for dashboard report
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ILogger<ReportController> _logger;
        private IReportRepository _reportRepository;
        private IGroupingRepository _groupingRepository;

        public ReportController(ILogger<ReportController> logger, IReportRepository reportRepository, IGroupingRepository groupingRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
            _groupingRepository = groupingRepository;
        }

        /// <summary>
        /// Returns a high level report for the product between the dates provided
        /// </summary>
        /// <param name="product"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("{product}")]
        public async Task<IActionResult> Get(string product, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var grouping = await _groupingRepository.GetAsync(product);

                var report = await _reportRepository.GetAsync(startDate, endDate, grouping);

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("500", ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
