using dotNetLabs.Blazor.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Server.Repositories
{
    public class CommentRepository : ICommentRepository
    {


        private readonly ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Comment  comment)
        {
            await _db.Comments.AddAsync(comment);
        }

        public IEnumerable<Comment> GetAllForVideo(string videoId)
        {
            return _db.Comments
                .Include(c => c.CreatedByUser)
                .Include(c => c.Replys)
                .ThenInclude(r => r.CreatedByUser)
                .Where(c => c.VideoId == videoId && c.ParentCommentId == null);
                
                
        }

        public async Task<Comment> GetCommentByIdAsync(string commentId)
        {
            return await _db.Comments
                .Include(c => c.CreatedByUser)
                .Include(c => c.Replys)
                .ThenInclude(r => r.CreatedByUser)
                .SingleOrDefaultAsync(c => c.Id == commentId);
        }

        public void Remove(Comment comment)
        {
            if(comment.Replys != null && comment.Replys.Any())
            {
                _db.Comments.RemoveRange(comment.Replys);
            }

            _db.Comments.Remove(comment);
        }
    }

}
