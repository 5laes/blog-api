using blogg_api.Data;
using blogg_api.Models;
using Microsoft.EntityFrameworkCore;

namespace blogg_api.Services
{
    public class BlogTagRepository : IAppRepository<BlogTag>
    {
        private readonly AppDbContext _context;

        public BlogTagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogTag> AddAsync(BlogTag newEntity)
        {
            if (_context.Tags.FirstOrDefault(t => t.TagName.ToLower() == newEntity.TagName.ToLower()) != null)
            {
                return null;
            }
            var result = await _context.Tags.AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<BlogTag> DeleteAsync(int Id)
        {
            var result = await _context.Tags.FirstOrDefaultAsync(t => t.Id == Id);
            if (result != null)
            {
                _context.Tags.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<BlogTag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<BlogTag> GetSingleAsync(int Id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<BlogTag> UpdateAsync(BlogTag newEntity)
        {
            var result = await _context.Tags.FirstOrDefaultAsync(t => t.Id == newEntity.Id);
            if (result != null)
            {
                result.TagName = newEntity.TagName;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
