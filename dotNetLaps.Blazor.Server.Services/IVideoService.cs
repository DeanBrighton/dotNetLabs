using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Server.Services.Utilities;
using dotNetLabs.Blazor.Shared;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public interface IVideoService
    {
        Task<OperationResponse<VideoDetail>> CreateAsync(VideoDetail model);
        Task<OperationResponse<VideoDetail>> UpdateAsync(VideoDetail model);
        Task<OperationResponse<VideoDetail>> RemoveAsyc(string id);
        CollectionResponse<VideoDetail> GetAllVideos(int pageNumber = 1, int pageSize = 10);
    }

    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identity;
        private readonly IFileStorageService _storage;



        public VideoService(IUnitOfWork unitOfWork, 
            IdentityOptions identity, 
            IFileStorageService storage)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
            _storage = storage;
        }



        public async Task<OperationResponse<VideoDetail>> CreateAsync(VideoDetail model)
        {

            var similarVideo = await _unitOfWork.Videos.GetByTitleAsync(model.Title);
            if (similarVideo != null)
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Message = "Another video has the same title"
                };

            if(model.ThumbFile == null)
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Message = "No thumbnail file provided."
                };



            string thumbUrl = string.Empty;
            try
            {
                thumbUrl = await _storage.SaveFileAsync(model.ThumbFile, "Uploads");

            }
            catch (BadImageFormatException)
            {
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Message = "Please upload a thumbnail image."
                };
            }

            var video = new Video
            {
                Category = model.Category,
                Description = model.Description,
                Likes = 0,
                Views = 0,
                ThumbURL = thumbUrl,
                Title = model.Title,
                VideoURL = model.VideoURL,
                PublishingDate = model.PublishingDate,
                VideoPrivacy = model.VideoPrivacy,
                Tags = model.Tags?.Select(t => new Tag
                {
                    Name = t,

                }).ToList(),


            };

            await _unitOfWork.Videos.CreateAsync(video);
            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            model.Id = video.Id;
            model.ThumbFile = null;
            model.ThumbURL = $"https://localhost:5001/{thumbUrl}";


            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Video has been created",
                Data = model
            };


        }

        public CollectionResponse<VideoDetail> GetAllVideos(int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<VideoDetail>> RemoveAsyc(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<VideoDetail>> UpdateAsync(VideoDetail model)
        {
            throw new NotImplementedException();
        }
    }
}
