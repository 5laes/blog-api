using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public string TagName { get; set; }

        public ICollection<BlogTag> Tags { get; set;} = new List<BlogTag>();
    }
}
