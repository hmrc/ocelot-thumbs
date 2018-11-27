using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        //not wanted as will output the entire database
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var result = await _thumbsRepository.GetManyAsync();
        //    return Ok(result);

        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _thumbsRepository.GetAsync(t => t.Id == id);

            if (item == null)
            {
                return NotFound(id);
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task Add([FromBody]Thumb item)
        {
            _thumbsRepository.Add(item);

            var saveResult = await _thumbsRepository.SaveAsync();

            //todo check saved and return result
            if (saveResult)
            {
            }
            else
            {
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]Thumb item)
        {
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
            //todo output message;
            return new StatusCodeResult(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

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
    }
}
