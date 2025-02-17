using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.NewsArticleDTOs
{
    public class GetNewsArticleDTO : BaseNewsArticleDTO
    {
        public short NewsArticleId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
