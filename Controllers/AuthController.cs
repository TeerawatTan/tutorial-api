using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tutorial_api_2.Dtos;
using tutorial_api_2.Services;

namespace tutorial_api_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public ActionResult<UserLoginDto> Login([FromBody] LoginDto dto)
        {
            if (dto is null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
                return BadRequest();

            UserLoginDto? userLoginDto = _authService.Login(dto);
            
            if (userLoginDto is null)
            {
                return NotFound();
            }

            return Ok(userLoginDto);
        }

        [HttpPost("Signup")]
        public IActionResult Signup([FromBody] SignUpDto dto)
        {
            SignUpDto? signUpDto = _authService.SignUp(dto);

            if (signUpDto is null)
            {
                return BadRequest();
            }

            return Ok(signUpDto);
        }
    }
}
