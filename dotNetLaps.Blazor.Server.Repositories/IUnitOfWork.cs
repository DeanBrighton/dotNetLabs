using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IPlaylistRepository Playlists { get; }
        IVideosRepository Videos { get; }

        ICommentRepository Comments { get; }

        Task CommitChangesAsync(string userId);

    }

}
