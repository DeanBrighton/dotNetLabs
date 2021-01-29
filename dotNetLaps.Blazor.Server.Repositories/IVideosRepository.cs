using dotNetLabs.Blazor.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public interface IVideosRepository
    {
        Task CreateAsync(Video video);

        void Remove(Video video);

        IEnumerable<Video> GetAll();

        Task<Video> GetByIdAsync(string id);

        Task<Video> GetByTitleAsync(string name);


    }

}
