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
    [Authorize(Roles = "Admin")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;

        }


        [ProducesResponseType(200, Type = typeof(OperationResponse<PlaylistDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<PlaylistDetail>))]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PlaylistDetail model)
        {
            var result = await _playlistService.CreateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<PlaylistDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<PlaylistDetail>))]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(PlaylistDetail model)
        {
            var result = await _playlistService.UpdateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<PlaylistDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<PlaylistDetail>))]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _playlistService.RemoveAsyc(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<VideoDetail>))]
        [HttpPost("AssignOrRemoveVideo")]
        public async Task<IActionResult> AssignOrRemoveFromPlaylist(PlaylistVideoRequest model)
        {
            var result = await _playlistService.AssignOrRemoveVideoFromPlaylistAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<PlaylistDetail>))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _playlistService.GetSinglePlaylistAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<PlaylistDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<PlaylistDetail>))]
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll(int pageNumber, int pageSize)
        {
            var result = _playlistService.GetAllPlaylists(pageNumber, pageSize);
            return Ok(result);

        }
    }
}
