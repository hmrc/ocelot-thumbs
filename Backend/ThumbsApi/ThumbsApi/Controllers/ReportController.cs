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

        internal ReportController(ILogger<ReportController> logger, IReportRepository reportRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
        }

        //[HttpGet()]
        //public IActionResult Get()
        //{
        //    return Ok($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        //}

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

                var report = await _reportRepository.GetAsync(startDate, endDate, product);

                //todo get grouping from api and loop through all children;
                //foreach (child in grouping.Children)
                //{
                //    report.Children.Add(_reportRepository.GetAsync(startDate, endDate, child.Product);)
                //}

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("500", ex.Message, ex);
                return StatusCode(500);

            }
        }
    }
}
