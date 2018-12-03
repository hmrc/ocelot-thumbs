using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Controllers
{
    /// <summary>
    /// todo write description
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ThumbsController : ControllerBase
    {
        private readonly IThumbsRepository _thumbsRepository;
        private readonly ILogger<ThumbsController> _logger;

        internal ThumbsController(IThumbsRepository thumbsRepository, ILogger<ThumbsController> logger)
        {
            _thumbsRepository = thumbsRepository;
            _logger = logger;
        }

        // not wanted as will output the entire database
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{            
        //    var result = await _thumbsRepository.GetManyAsync();           
        //    return Ok(result);
        //}

        /// <summary>
        /// todo write description
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name=nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var item = await _thumbsRepository.GetAsync(t => t.Id == id);

                if (item == null)
                {
                    return NotFound(id);
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// todo write description
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Thumb item)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                item.Pid = User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1); ;

                _thumbsRepository.Add(item);

                if (await _thumbsRepository.SaveAsync())
                {
                    return CreatedAtRoute(nameof(GetById), new { id = item.Id }, item);
                }
                else
                {
                    return StatusCode(500, "Save to database failed");
                }                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// todo write description
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]Thumb item)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var result = await _thumbsRepository.GetAsync(t => t.Id == id);

                if (result == null)
                {
                    return NotFound(id);
                }

                result.Rating = item.Rating;
                result.Pid = item.Pid;
                result.Group = item.Group;

                _thumbsRepository.Update(result);

                if (await _thumbsRepository.SaveAsync())
                {
                    return NoContent();
                }
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// todo write description
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();
                }

                var result = await _thumbsRepository.GetAsync(t => t.Id == id);

                if (result == null)
                {
                    return NotFound(id);
                }

                _thumbsRepository.Delete(result);

                if (await _thumbsRepository.SaveAsync())
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "Failed to delete");
                }
            }
            catch (Exception ex)
            {

                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
