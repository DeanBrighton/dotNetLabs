using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Mappers;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public interface IPlaylistService
    {
        Task<OperationResponse<PlaylistDetail>> CreateAsync(PlaylistDetail model);
        Task<OperationResponse<PlaylistDetail>> UpdateAsync(PlaylistDetail model);
        Task<OperationResponse<PlaylistDetail>> RemoveAsyc(string id);
        CollectionResponse<PlaylistDetail> GetAllPlaylists(int pageNumber = 1, int pageSize = 10);

    }

    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityOptions _identity;

        public PlaylistService(IUnitOfWork unitOfWork, IdentityOptions identity)
        {
            _unitOfWork = unitOfWork;
            _identity = identity;
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

 
    }

}
