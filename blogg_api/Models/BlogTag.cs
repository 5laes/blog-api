using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace blogg_api.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public string TagName { get; set; }

        [JsonIgnore]
        public ICollection<BlogPost> Posts { get; set;}
    }
}
