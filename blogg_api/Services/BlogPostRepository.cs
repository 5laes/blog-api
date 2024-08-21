using blogg_api.Data;
using blogg_api.Models;

namespace blogg_api.Services
{
    public class BlogPostRepository : IAppRepository<BlogPost>
    {
        AppDbContext _context;

        public BlogPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<BlogPost> AddAsync(BlogPost newEntity)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
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
