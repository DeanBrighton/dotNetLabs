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
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
            
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<CommentDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<CommentDetail>))]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CommentDetail model)
        {
            var result = await _commentService.CreateAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<CommentDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<CommentDetail>))]
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(CommentDetail model)
        {
            var result = await _commentService.EditAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<CommentDetail>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<CommentDetail>))]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _commentService.RemoveAsyc(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }



    }
}
