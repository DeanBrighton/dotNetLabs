using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Server.Mappers;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public class CommentService : ICommentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identity;

        public CommentService(IUnitOfWork unitOfWork, IdentityOptions identity)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
        }

        public async Task<OperationResponse<CommentDetail>> CreateAsync(CommentDetail model)
        {
            //Parent comment check
            Comment parentComment = null;
            if (!string.IsNullOrWhiteSpace(model.ParentCommentId))
            {
                parentComment = await _unitOfWork.Comments.GetCommentByIdAsync(model.ParentCommentId);
                if (parentComment == null)
                    return new OperationResponse<CommentDetail>
                    {
                        IsSuccess = false,
                        Message = "Parent comment cannot be found"
                    };
            }




            //Video available check

            if (string.IsNullOrWhiteSpace(model.VideoId))
                return new OperationResponse<CommentDetail> { IsSuccess = false, Message = "Video ID is required" };

            var video = await _unitOfWork.Videos.GetByIdAsync(model.VideoId);
            if (video == null)
                return new OperationResponse<CommentDetail>
                {
                    IsSuccess = false,
                    Message = "Video cannot be found"
                };



            var newComment = new Comment
            {
                Content = model.Content,
                Likes = 0,
                ParentComment = parentComment,
                Video = video,
            };

            await _unitOfWork.Comments.CreateAsync(newComment);
            await _unitOfWork.CommitChangesAsync(_identity.UserID);
            model.Id = newComment.Id;
            model.CommentDate = newComment.CreationDate;

            return new OperationResponse<CommentDetail>
            {
                IsSuccess = true,
                Message = "Comment has been saved",
                Data = model
            };


        }

        public async Task<OperationResponse<CommentDetail>> EditAsync(CommentDetail model)
        {
            var comment = await _unitOfWork.Comments.GetCommentByIdAsync(model.Id);
            if (comment == null)
                return new OperationResponse<CommentDetail>
                {
                    IsSuccess = false,
                    Message = "Comment cannot be found"
                };

            comment.Content = model.Content;
            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<CommentDetail>
            {
                IsSuccess = true,
                Message = "Comment has been edited",
                Data = model
            };

        }

        public IEnumerable<CommentDetail> GetVideoComments(string videoId)
        {
            var comments = _unitOfWork.Comments.GetAllForVideo(videoId);

            return comments.Select(c => c.ToCommentDetail());
        }

        public async Task<OperationResponse<CommentDetail>> RemoveAsyc(string commentId)
        {
            var comment = await _unitOfWork.Comments.GetCommentByIdAsync(commentId);
            if (comment == null)
                return new OperationResponse<CommentDetail>
                {
                    IsSuccess = false,
                    Message = "Comment cannot be found"
                };

            _unitOfWork.Comments.Remove(comment);
            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<CommentDetail>
            {
                IsSuccess = true,
                Message = "Comment has been deleted",
                Data = comment.ToCommentDetail()
            };

        }
    }

}
