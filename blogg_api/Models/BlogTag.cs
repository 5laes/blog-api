using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models
{
    public class BlogTag
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }
    }
}
