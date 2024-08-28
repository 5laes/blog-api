using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models.DTOs
{
    public class BlogContentCreateDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int TagId { get; set; }
    }
}
