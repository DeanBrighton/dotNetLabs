using dotNetLabs.Blazor.Server.Mappers;
using dotNetLabs.Blazor.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Services
{
    public interface ICommentService
    {
        Task<OperationResponse<CommentDetail>> CreateAsync(CommentDetail model);
        Task<OperationResponse<CommentDetail>> EditAsync(CommentDetail model);
        Task<OperationResponse<CommentDetail>> RemoveAsyc(string commentId);
        IEnumerable<CommentDetail> GetVideoComments(string videoId);

    }

}
