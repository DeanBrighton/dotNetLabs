using dotNetLabs.Blazor.Shared;
using Microsoft.Extensions.Hosting;
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
}
