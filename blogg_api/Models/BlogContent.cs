using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace blogg_api.Models
{
    public class BlogContent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string YouTubeLink { get; set; }
        public string GitHubLink { get; set; }
        public DateTime DatePublished { get; set; }

        [JsonIgnore]
        public ICollection<BlogPost> Posts { get; set; }

        // TODO: add image to post
    }
}
