using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Mappers;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace dotNetLabs.Blazor.Server.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identity;
        private readonly IMapper _mapper;

        public PlaylistService(IUnitOfWork unitOfWork, IdentityOptions identity, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
            _mapper = mapper;
        }

        public async Task<OperationResponse<PlaylistDetail>> CreateAsync(PlaylistDetail model)
        {

            var playlist = new Playlist
            {
                Name = model.Name,
                Description = model.Description

            };

            await _unitOfWork.Playlists.CreateAsync(playlist);
            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            model.Id = playlist.Id;

            return new OperationResponse<PlaylistDetail>
            {
                IsSuccess = true,
                Message = "Playlist created",
                Data = model
            };

        }

        public CollectionResponse<PlaylistDetail> GetAllPlaylists(int pageNumber = 1, int pageSize = 10)
        {

            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 5)
                pageSize = 5;

            if (pageSize > 50)
                pageSize = 50;

            var playlists = _unitOfWork.Playlists.GetAll();
            int playlistcount = playlists.Count();

            var playlistsInPage = playlists
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => p.ToPlayListDetail()) ;


            int pagesCount = playlistcount / pageSize;
            if ((playlistcount % pageSize) != 0)
                pagesCount++;



            return new CollectionResponse<PlaylistDetail>
            {
                IsSuccess = true,
                Message = "Playlists retreived successfully",
                Records = playlistsInPage.ToArray(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                PageCount = pagesCount
            };

        }

        public async Task<OperationResponse<PlaylistDetail>> RemoveAsyc(string id)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null)
                return new OperationResponse<PlaylistDetail>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Playlist not found"
                };

             _unitOfWork.Playlists.Remove(playlist);

            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<PlaylistDetail>
            {
                IsSuccess = true,
                Message = "Playlist has been removed",
                Data = playlist.ToPlayListDetail()
            };


        }

        public async Task<OperationResponse<PlaylistDetail>> UpdateAsync(PlaylistDetail model)
        {

            var playlist = await _unitOfWork.Playlists.GetByIdAsync(model.Id);
            if (playlist == null)
                return new OperationResponse<PlaylistDetail>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Playlist not found"
                };

            playlist.Name = model.Name;
            playlist.Description = model.Description;

            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<PlaylistDetail>
            {
                IsSuccess = true,
                Message = "Playlist has been updated",
                Data = model
            };

        }

        public async Task<OperationResponse<VideoDetail>> AssignOrRemoveVideoFromPlaylistAsync(PlaylistVideoRequest request)
        {

            var video = await _unitOfWork.Videos.GetByIdAsync(request.VideoId);
            if (video == null)
                return new OperationResponse<VideoDetail> { IsSuccess = false, Message = "Playlist video request. Video cannot be found" };

            var playlist = await _unitOfWork.Playlists.GetByIdAsync(request.PlaylistId);
            if (playlist == null)
                return new OperationResponse<VideoDetail> { IsSuccess = false, Message = "Playlist video request. Playlist cannot be found" };

            //var playlistVideos = _unitOfWork.Playlists.GetAllVideosInPlaylist(request.PlaylistId);
            var playlistVideo = playlist.PlaylistVideos.SingleOrDefault(pv => pv.VideoID == request.VideoId);

            string message = string.Empty;

            if (playlistVideo != null)
            {
                //remove

                _unitOfWork.Playlists.RemoveVideoFromPlaylist(playlistVideo);


                message = "Video has been removed from the playlist";

            }
            else
            {
                //add
                await _unitOfWork.Playlists.AddVideoToPlaylistAsync(new PlaylistVideo
                {
                    Playlist = playlist,
                    Video = video

                });
                await _unitOfWork.CommitChangesAsync(_identity.UserID);
                message = "Video has been ADDED to the playlist";

            }

            await _unitOfWork.CommitChangesAsync(_identity.UserID);

            return new OperationResponse<VideoDetail>
            {
                IsSuccess = true,
                Message = message
            };


        }

        public async Task<OperationResponse<PlaylistDetail>> GetSinglePlaylistAsync(string id)
        {

            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null)
                return new OperationResponse<PlaylistDetail> { IsSuccess = false, Message = "Playlist video request. Playlist cannot be found" };
            var videos = playlist.PlaylistVideos?.Select(pv => _mapper.Map<VideoDetail>(pv.Video)).ToArray();
            return new OperationResponse<PlaylistDetail>
            {
                Data = playlist.ToPlayListDetail(videos,true),
                Message = "Playlist has been retrieved",
                IsSuccess = true
            };

        }
    }

}
