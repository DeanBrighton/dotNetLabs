﻿using dotNetLabs.Blazor.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;


        public EfUnitOfWork(UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager,
                                       ApplicationDbContext db)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;

        }

        private IUsersRepository _users;
        public IUsersRepository Users 
        {
            get
            {
                if (_users == null)
                    _users = new IdentityUsersRepository(_userManager, _roleManager);

                return _users;
            }
        }

        private IPlaylistRepository _playlists;
        public IPlaylistRepository Playlists
        {
            get
            {
                if (_playlists == null)
                    _playlists = new PlaylistsRepository(_db);

                return _playlists;
            }
        }

        private IVideosRepository _videos;
        public IVideosRepository Videos
        {
            get
            {
                if (_videos == null)
                    _videos = new VideosRepository(_db);

                return _videos;
            }
        }

        private ICommentRepository _comments;
        public ICommentRepository Comments
        {
            get
            {
                if (_comments == null)
                    _comments = new CommentRepository(_db);

                return _comments;
            }
        }


        public async Task CommitChangesAsync(string userId)
        {
            
            await _db.SaveChangesAsync(userId);
        }
    }

}
