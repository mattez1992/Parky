using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DTOS;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    public class NationalParkControllerVTwo : ControllerBase
    {
        private readonly INationalParkRepo _nationalParkRepo;

        public NationalParkControllerVTwo(INationalParkRepo nationalParkRepo)
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
       
    }
}
