using Microsoft.AspNetCore.Mvc;
using NoteStorage.Jwt.Interfaces;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Web.Models;
using System.Threading.Tasks;

namespace NoteStorage.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtTokenService jwtTokenService;

        public LoginController(IUserService usersService, IJwtTokenService jwtTokenService)
        {
            this.userService = usersService;
            this.jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserModel userModel)
        {
            var user = await userService.AuthenticateAsync(userModel.Login, userModel.Password);

            if (user != null)
            {
                var token = jwtTokenService.GenerateToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
