using blogg_api.Data;
using blogg_api.Models;
using Microsoft.EntityFrameworkCore;

namespace blogg_api.Services
{
    public class BlogContentRepository : IAppRepository<BlogContent>
    {
        private readonly AppDbContext _context;
        public BlogContentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<BlogContent> AddAsync(BlogContent newEntity)
        {
            var result = await _context.Contents.AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<BlogContent> DeleteAsync(int Id)
        {
            var entityToDelete = await _context.Contents.FirstOrDefaultAsync(c => c.Id == Id);
            if (entityToDelete != null)
            {
                var result = _context.Contents.Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<IEnumerable<BlogContent>> GetAllAsync()
        {
            return await _context.Contents.ToListAsync();
        }

        public async Task<BlogContent> GetSingleAsync(int Id)
        {
            return await _context.Contents.FirstOrDefaultAsync(c => c.Id == Id); // fix check for id not found
        }

        public async Task<BlogContent> UpdateAsync(BlogContent newEntity)
        {
            var result = await _context.Contents.FirstOrDefaultAsync(c => c.Id == newEntity.Id);
            if (result != null)
            {
                result.Title = newEntity.Title;
                result.Content = newEntity.Content;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
