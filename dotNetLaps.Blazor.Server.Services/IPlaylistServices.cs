using dotNetLabs.Blazor.Server.Infrastructure;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Server.Repositories;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public interface IPlaylistService
    {
        Task<OperationResponse<PlaylistDetail>> CreateAsync(PlaylistDetail model);
        Task<OperationResponse<PlaylistDetail>> UpdateAsync(PlaylistDetail model);
        Task<OperationResponse<PlaylistDetail>> RemoveAsyc(string id);
        //Task<OperationResponse<PlaylistDetail>> CreateAsync(PlaylistDetail model);



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

        public Task<OperationResponse<PlaylistDetail>> RemoveAsyc(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResponse<PlaylistDetail>> UpdateAsync(PlaylistDetail model)
        {
            throw new NotImplementedException();
        }
    }

}
