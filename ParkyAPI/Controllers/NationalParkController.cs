using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DTOS;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "NationalParks")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepo _nationalParkRepo;

        public NationalParkController(INationalParkRepo nationalParkRepo)
        {
            _nationalParkRepo = nationalParkRepo;
        }
        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public async Task<ActionResult<List<NationalParkDto>>> GetNationalParks()
        {
            var response = await _nationalParkRepo.GetAllParks();
            return Ok(response);
        }
        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="id"> The Id of the national Park </param>
        [HttpGet("{id:int}", Name = "GetNationalParkByIdAsync")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<NationalParkDto>> GetNationalParkByIdAsync(int id)
        {
            var response = await _nationalParkRepo.GetParkById(id);
            return Ok(response);
        }
        /// <summary>
        /// Create a national park
        /// </summary>
        /// <param name="park">The dto required to create a park</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePark([FromBody] NationalParkDto park)
        {
            if(park == null)
            {
                return BadRequest(ModelState);
            }
            if (_nationalParkRepo.NationalParkExists(park.Name).GetAwaiter().GetResult())
            {
                ModelState.AddModelError("", "This name already exists");
                return StatusCode(404,ModelState);
            }
            var response = await _nationalParkRepo.CreateNationalPark(park);
            if(!response)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {park.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalParkByIdAsync", new { version=HttpContext.GetRequestedApiVersion().ToString(), id = park.Id }, park);
        }
        /// <summary>
        /// Updates a park
        /// </summary>
        /// <param name="id">The id of the park to update</param>
        /// <param name="park">The object holding the updates</param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNationalPark(int id, [FromBody] NationalParkDto park)
        {
            if (park == null || id != park.Id)
            {
                return BadRequest(ModelState);
            }
            var response = await _nationalParkRepo.UpdateNationalPark(park);
            if (!response)
            {
                ModelState.AddModelError("", $"Something went wrong when updateing the record {park.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// Deletes a park
        /// </summary>
        /// <param name="id">The id of the park to delete</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNationalPark(int id)
        {
            if(!(await _nationalParkRepo.NationalParkExists(id)))
            {
                return NotFound();
            }
            
            var response = await _nationalParkRepo.DeleteNationalPark(id);
            if (!response)
            {
                ModelState.AddModelError("", $"Something went wrong when deleteing the record");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
