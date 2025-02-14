using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.SystemAccountDTOs;

namespace NewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public AuthController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _systemAccountService.Login(loginDTO);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


    }
}
