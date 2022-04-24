using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DTOS;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/trails")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "Trails")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepo _trailRepo;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepo trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }
       /// <summary>
       /// Gets a list of trails
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ReadTrailDto>))]
        public async Task<ActionResult<List<ReadTrailDto>>> GetTrails()
        {
            var response = await _trailRepo.GetAllTrails();
            return Ok(response);
        }
        /// <summary>
        /// Get one trail based on it's id
        /// </summary>
        /// <param name="id">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrailByIdAsync")]
        [ProducesResponseType(200, Type = typeof(ReadTrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ReadTrailDto>> GetTrailByIdAsync(int id)
        {
            var response = await _trailRepo.GetTrailById(id);
            return Ok(response);
        }
        /// <summary>
        /// Gets all the trails in a park 
        /// </summary>
        /// <param name="nationalParkId">The parks id</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(ReadTrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetTrailInNationalPark(int nationalParkId)
        {
            var trails = await _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (trails == null)
            {
                return NotFound();
            }
            var trailDtos = new List<ReadTrailDto>();
            foreach (var trail in trails)
            {
                trailDtos.Add(_mapper.Map<ReadTrailDto>(trail));
            }


            return Ok(trailDtos);

        }
        /// <summary>
        /// Create a trail
        /// </summary>
        /// <param name="trailDto">The create dto</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReadTrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTrail([FromBody] CreateTrailDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name).GetAwaiter().GetResult())
            {
                ModelState.AddModelError("", "This name already exists");
                return StatusCode(404, ModelState);
            }
            var newTrail = _mapper.Map<Trail>(trailDto);
            var response = await _trailRepo.CreateTrail(newTrail);
            if (!response)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {newTrail.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrailByIdAsync", new { id = newTrail.Id }, newTrail);
        }
        /// <summary>
        /// Updates a trail
        /// </summary>
        /// <param name="id">The id of the trail to update</param>
        /// <param name="trail">The dto to update a trail</param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrail(int id, [FromBody] TrailUpdateDto trail)
        {
            if (trail == null || id != trail.Id)
            {
                return BadRequest(ModelState);
            }
            var response = await _trailRepo.UpdateTrail(trail);
            if (!response)
            {
                ModelState.AddModelError("", $"Something went wrong when updateing the record {trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// Deletes a Trail
        /// </summary>
        /// <param name="id">The Id of the trail</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrail(int id)
        {
            if (!(await _trailRepo.TrailExists(id)))
            {
                return NotFound();
            }

            var response = await _trailRepo.DeleteTrail(id);
            if (!response)
            {
                ModelState.AddModelError("", $"Something went wrong when deleteing the record");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
