using dotNetLabs.Blazor.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public interface IPlaylistRepository
    {
        Task CreateAsync(Playlist playlist);

        void Remove(Playlist playlist);

        IEnumerable<Playlist> GetAll();

        Task<Playlist> GetByIdAsync(string id);

        public IEnumerable<Video> GetAllVideosInPlaylist(string id);

        public void RemoveVideoFromPlaylist(PlaylistVideo playlistVideo);
        public Task AddVideoToPlaylistAsync(PlaylistVideo playlistVideo);

    }

}
