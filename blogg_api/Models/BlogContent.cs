﻿using System.ComponentModel.DataAnnotations;

namespace blogg_api.Models
{
    public class BlogContent
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePublished { get; set; }

        // TODO: add image to post
    }
}
