namespace blogg_api.Models.DTOs
{
    public class BlogPostWithTagsDTO
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
