using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models.DTOs
{
    public class BlogTagUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string TagName { get; set; }
    }
}
