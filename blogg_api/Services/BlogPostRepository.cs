using blogg_api.Data;
using blogg_api.Models;
using Microsoft.EntityFrameworkCore;

namespace blogg_api.Services
{
    public class BlogPostRepository : IAppRepository<BlogPost>, IPostRepository<BlogPost>
    {
        AppDbContext _context;

        public BlogPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPost> AddAsync(BlogPost newEntity)
        {
            var result = await _context.Posts.AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<BlogPost> DeleteAsync(int Id)
        {
            var posts = await _context
                .Posts
                .Where(x => x.ContentId == Id)
                .Include(x => x.Content)
                .Include(x => x.Tag)
                .ToListAsync();
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                }
                return posts.FirstOrDefault();
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var result = await _context.Posts
                .Include(x => x.Content)
                .Include(t => t.Tag)
                .ToListAsync();

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<BlogPost>> GetPostsByTagAsync(int tagId)
        {
            var result = await _context
                .Posts
                .Where(x => x.TagId == tagId)
                .Include(x => x.Content)
                .Include(x => x.Tag)
                .ToListAsync();

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<BlogPost>> GetPostWithTagsAsync(int postId)
        {
            var result = await _context.Posts
                .Where(x => x.ContentId == postId)
                .Include(x => x.Content)
                .Include(x => x.Tag)
                .ToListAsync();

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public Task<BlogPost> GetSingleAsync(int Id)
        {
            throw new NotImplementedException();
        }

        // Figure out how to remove a tag from a post
        public async Task<BlogPost> RemoveTagAsync(int postId, int tagId)
        {
            var post = await _context.Posts.Where(x => x.ContentId == postId).FirstOrDefaultAsync(x => x.TagId == tagId);
            if (post == null)
            {
                return null;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public Task<BlogPost> UpdateAsync(BlogPost newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
