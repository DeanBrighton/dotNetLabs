using dotNetLabs.Blazor.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(Comment comment);

        void Remove(Comment comment);

        IEnumerable<Comment> GetAllForVideo(string videoId);

        Task<Comment> GetCommentByIdAsync(string commentId);


    }

}
