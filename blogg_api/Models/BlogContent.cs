using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models
{
    public class BlogContent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }

        public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();

        // TODO: add image to post
    }
}
