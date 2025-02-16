using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.NewsArticleDTOs
{
    public class BaseNewsArticleDTO
    {
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string NewsSource { get; set; } = string.Empty;
        public bool NewsStatus { get; set; }
        public DateTime ModifiedName { get; set; }

    }
}
