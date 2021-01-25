using dotNetLabs.Blazor.Server.Services;
using dotNetLabs.Blazor.Shared;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;

        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PlaylistDetail model)
        {
            var result = await _playlistService.CreateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(int pageNumber, int pageSize)
        {
            var result = _playlistService.GetAllPlaylists(pageNumber, pageSize);
            return Ok(result);

        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(PlaylistDetail model)
        {
            var result = await _playlistService.UpdateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _playlistService.RemoveAsyc(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }


    }
}
