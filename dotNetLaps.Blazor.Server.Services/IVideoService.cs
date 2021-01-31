using AutoMapper;
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
        CollectionResponse<VideoDetail> GetAllVideos(string query, int pageNumber = 1, int pageSize = 10);

        Task<OperationResponse<VideoDetail>> GetVideoDetailAsync(string videoId);


    }

    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identity;
        private readonly IFileStorageService _storage;
        private readonly EnvironmentOptions _env;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;


        public VideoService(IUnitOfWork unitOfWork, 
            IdentityOptions identity, 
            IFileStorageService storage,
            EnvironmentOptions env,
            IMapper mapper,
            ICommentService commentService)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
            _storage = storage;
            _env = env;
            _mapper = mapper;
            _commentService = commentService;
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
            model.ThumbURL = $"{_env.ApiUrl}/{thumbUrl}";


            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Video has been created",
                Data = model
            };


        }

        public CollectionResponse<VideoDetail> GetAllVideos(string query, int pageNumber = 1, int pageSize = 10)
        {

            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 5)
                pageSize = 5;

            if (pageSize > 50)
                pageSize = 50;

            var videos = _unitOfWork.Videos.GetAll();

            int videoscount = videos.Count();

            var videosInPage = videos
                .Where(v => v.Title.Contains(query,StringComparison.InvariantCultureIgnoreCase)
                            || v.Description.Contains(query,StringComparison.InvariantCultureIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<VideoDetail>(p));


            int pagesCount = videoscount / pageSize;
            if ((videoscount % pageSize) != 0)
                pagesCount++;

            return new CollectionResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Videos retreived successfully",
                Records = videosInPage.ToArray(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                PageCount = pagesCount
            };


        }

        public async Task<OperationResponse<VideoDetail>> GetVideoDetailAsync(string videoId)
        {
            var video = await _unitOfWork.Videos.GetByIdAsync(videoId);
            if (video == null)
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Message = "Video cannot be found"
                };


            var videoDetail = _mapper.Map<VideoDetail>(video);

            //get all the comments

            videoDetail.Comments = _commentService.GetVideoComments(videoId);


            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Video information retrieved",
                Data = videoDetail
            };


        }

        public async Task<OperationResponse<VideoDetail>> RemoveAsyc(string id)
        {
            var video = await _unitOfWork.Videos.GetByIdAsync(id);
            if (video == null)
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Video not found"
                };

            _unitOfWork.Videos.RemoveTags(video);
            //TODO: Remove comments and playlist assignments

            _unitOfWork.Videos.Remove(video);
            _storage.RemoveFile(video.ThumbURL);


            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Video has been removed",
                Data = _mapper.Map<VideoDetail>(video),
            };

        }

        public async Task<OperationResponse<VideoDetail>> UpdateAsync(VideoDetail model)
        {

            var video = await _unitOfWork.Videos.GetByIdAsync(model.Id);
            if (video == null)
                return new OperationResponse<VideoDetail>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Video not found"
                };

            //Check the thumb image
            var thumbUrl = video.ThumbURL;
            if(model.ThumbFile != null)
            {
                try
                {
                    thumbUrl = await _storage.SaveFileAsync(model.ThumbFile, "Uploads");
                    _storage.RemoveFile(video.ThumbURL);

                }
                catch (BadImageFormatException)
                {
                    return new OperationResponse<VideoDetail>
                    {
                        IsSuccess = false,
                        Message = "Please upload a thumbnail image."
                    };
                }
            }

            _unitOfWork.Videos.RemoveTags(video);

            video.Title = model.Title;
            video.Description = model.Description;
            video.VideoPrivacy = model.VideoPrivacy;
            video.Category = model.Category;
            video.PublishingDate = model.PublishingDate;
            video.ThumbURL = thumbUrl;
            video.Tags = model.Tags?.Select(t => new Tag
            {
                Name = t,

            }).ToList();


            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            model.ThumbFile = null;
            model.ThumbURL = $"{_env.ApiUrl}/{thumbUrl}";

            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = "Video has been updated",
                Data = model
            };

        }
    }
}
