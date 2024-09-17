using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models.DTOs
{
    public class BlogContentUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string YoutubeLink { get; set; }
        public string GitHubLink { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
