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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = CLAES; Database = blog_api_db; Encrypt = False; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogContent>().HasData(
                new BlogContent()
                {
                    Id = 1,
                    Title = "Test",
                    Content = "This is a test blog",
                    DatePublished = DateTime.Now
                });

            modelBuilder.Entity<BlogTag>().HasData(
                new BlogTag()
                {
                    Id = 1,
                    TagName = "Test",
                });

            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost()
                {
                    ContentId = 1,
                    TagId = 1,
                });

        }
    }
}
