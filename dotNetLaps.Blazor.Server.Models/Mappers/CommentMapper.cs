using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetLabs.Blazor.Server.Mappers
{
    public static class CommentMapper
    {
        public static CommentDetail ToCommentDetail(this Comment comment)
        {
            return new CommentDetail
            {
                CommentDate = comment.CreationDate,
                Id = comment.Id,
                Content = comment.Content,
                ParentCommentId = comment.ParentCommentId,
                Username = $"{comment.CreatedByUser.FirstName} {comment.CreatedByUser.LastName}",
                VideoId = comment.VideoId,
                Replys = comment.Replys?.Select(c => c.ToCommentDetail())

            };
        }
    }
}
