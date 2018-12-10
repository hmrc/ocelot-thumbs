using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ThumbsApi.Models;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi.Controllers
{
    /// <summary>
    /// main controller class for thumbs to add,delete,update and search purposes 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ThumbsController : ControllerBase
    {
        private readonly IThumbsRepository _thumbsRepository;
        private readonly IDeletionRepository _deletionRepository;
        private readonly ILogger<ThumbsController> _logger;

        public ThumbsController(ILogger<ThumbsController> logger,
                                  IThumbsRepository thumbsRepository, 
                                  IDeletionRepository deletionRepository)
        {
            _thumbsRepository = thumbsRepository;
            _deletionRepository = deletionRepository;
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
        /// Returns thumb ,required ID parameter
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
        /// to add thumb into database, required pid, date, rating, product, group and endpoint parameters
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

                item.Pid = GetPid();

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
        /// to update thumb entry, required id to fetch the reocrd form the database
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
                result.Group = item.Group;

                _thumbsRepository.Update(result);

                if (await _thumbsRepository.SaveAsync())
                {
                    return NoContent();
                   // return CreatedAtRoute(nameof(GetById), new { id = item.Id }, item);
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
        /// to delete thumb from the database, required id(Guid) parameter to select the record from the database
        /// and also add user's PID in deletion table for audit purposes
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

                _deletionRepository.Add(new Deletion(result)
                {
                    DeletedBy = GetPid()
                });

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

        private string GetPid()
        {
            return User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1);
        }
    }
}
