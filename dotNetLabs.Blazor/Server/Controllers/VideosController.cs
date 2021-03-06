﻿using dotNetLabs.Blazor.Server.Services;
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
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
                
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<VideoDetail>))]
        [HttpPost("Create")]
        public async Task<IActionResult>Create ([FromForm]VideoDetail model)
        {
            var result = await _videoService.CreateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);

        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<VideoDetail>))]
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll(string query="", int pageNumber = 1, int pageSize=10)
        {
            var result =  _videoService.GetAllVideos(query,pageNumber, pageSize);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideo(string id)
        {
            var result = await _videoService.GetVideoDetailAsync(id);
            if (!result.IsSuccess)
                return NotFound();
            return Ok(result);
        }


        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<VideoDetail>))]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] VideoDetail model)
        {
            var result = await _videoService.UpdateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<VideoDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<VideoDetail>))]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _videoService.RemoveAsyc(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }



    }
}
