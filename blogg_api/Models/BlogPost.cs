using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogg_api.Models
{
    [PrimaryKey(nameof(ContentId), nameof(TagId))]
    public class BlogPost
    {
        [Key]
        [Column(Order = 1)]
        public int ContentId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int TagId { get; set; } 
    }
}
