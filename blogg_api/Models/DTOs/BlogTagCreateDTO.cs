using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models.DTOs
{
    public class BlogTagCreateDTO
    {
        [Required]
        public string TagName { get; set; }
    }
}
