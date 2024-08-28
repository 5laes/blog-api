using blogg_api.Data;
using blogg_api.Models;
using Microsoft.EntityFrameworkCore;

namespace blogg_api.Services
{
    public class BlogPostRepository : IAppRepository<BlogPost>
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

        public Task<BlogPost> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.Posts.Include(x => x.Content).Include(t => t.Tag).ToListAsync();
        }

        public Task<BlogPost> GetSingleAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> UpdateAsync(BlogPost newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
