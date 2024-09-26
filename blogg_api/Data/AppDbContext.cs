using Microsoft.EntityFrameworkCore;
using blogg_api.Models;

namespace blogg_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<BlogContent> Contents { get; set; }
        public DbSet<BlogTag> Tags { get; set; }
        public DbSet<BlogPost> Posts { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server = CLAES; Database = blog_api_db; Encrypt = False; Trusted_Connection = True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
