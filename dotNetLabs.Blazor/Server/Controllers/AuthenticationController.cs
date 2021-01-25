using dotNetLabs.Blazor.Server.Services;
using dotNetLabs.Blazor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUsersService _userService;

        public AuthenticationController(IUsersService usersService)
        {
            _userService = usersService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var result = await _userService.GenerateTokenAsync(model);
            if(result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _userService.RegisterUserAsync(model);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}
