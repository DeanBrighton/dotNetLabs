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
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
                
        }

        [HttpPost("Create")]
        public async Task<IActionResult>Create ([FromForm]VideoDetail model)
        {
            var result = await _videoService.CreateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);

        }

    }
}
