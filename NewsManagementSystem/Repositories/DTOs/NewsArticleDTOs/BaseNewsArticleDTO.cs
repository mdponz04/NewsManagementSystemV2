﻿using Data.Entities;

namespace Repositories.DTOs.NewsArticleDTOs
{
    public class BaseNewsArticleDTO
    {
        public short CategoryId { get; set; }
        public bool NewsStatus { get; set; }
        public string NewsTitle { get; set; } = null!;
        public string Headline { get; set; } = null!;
        public string NewsContent { get; set; } = null!;
        public string NewsSource { get; set; } = null!;
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
