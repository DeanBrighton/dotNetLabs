using dotNetLabs.Blazor.Shared;
using System;
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
        Task<OperationResponse<VideoDetail>> AssignOrRemoveVideoFromPlaylistAsync(PlaylistVideoRequest request);

        Task<OperationResponse<PlaylistDetail>> GetSinglePlaylistAsync(string id);


    }

}
