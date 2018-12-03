using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ThumbsController : ControllerBase
    {
        private readonly IThumbsRepository _thumbsRepository;
        private readonly ILogger<ThumbsController> _logger;

        public ThumbsController(IThumbsRepository thumbsRepository, ILogger<ThumbsController> logger)
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

                var saveResult = await _thumbsRepository.SaveAsync();

                //todo check saved
                return CreatedAtRoute(nameof(GetById), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500);
            }
        }

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

                //todo check if saved correctly
                var saveResult = await _thumbsRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogCritical(500, ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
