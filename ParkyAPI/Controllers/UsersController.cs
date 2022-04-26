using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DTOS;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepository;

        public UsersController(IUserRepo userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="loginDro">Model for holding Login credentials</param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(UserReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDro loginDro)
        {
            var user = await _userRepository.Login(loginDro.Username, loginDro.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
           
            return Ok(user);
        }
        /// <summary>
        /// Registers a user and sends it back
        /// </summary>
        /// <param name="loginDro">Model for holding Creation credentials</param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(UserReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] LoginDro loginDro)
        {
            var userExists = await _userRepository.UserExists(loginDro.Username);
            if (userExists)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = await _userRepository.Register(loginDro.Username, loginDro.Password);
            if (user == null)
            {

                return BadRequest(new { message = "Error while registering" });
            }
            return Ok(user);
        }
    }
}
