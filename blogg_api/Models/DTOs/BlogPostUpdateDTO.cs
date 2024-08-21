using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models.DTOs
{
    public class BlogPostUpdateDTO
    {
        [Required]
        public int ContentId { get; set; }
        [Required]
        public int TagId { get; set; }
    }
}
