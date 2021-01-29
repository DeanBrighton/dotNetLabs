﻿using dotNetLabs.Blazor.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public class PlaylistsRepository : IPlaylistRepository
    {

        private readonly ApplicationDbContext _db;

        public PlaylistsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Playlist playlist)
        {
            await _db.Playlists.AddAsync(playlist);
        }

        public IEnumerable<Playlist> GetAll()
        {
            return _db.Playlists;
        }

        public async Task<Playlist> GetByIdAsync(string id)
        {
            return await _db.Playlists.FindAsync(id);
        }

        public void Remove(Playlist playlist)
        {
            _db.Playlists.Remove(playlist);
        }




    }

}