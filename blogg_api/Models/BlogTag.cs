using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace blogg_api.Models
{
    public class BlogTag
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }

        [JsonIgnore]
        public ICollection<BlogContent> Contents { get; set;}
    }
}
