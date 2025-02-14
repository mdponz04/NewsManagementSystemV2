using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.NewsArticleDTOs
{
    public class GetNewsArticleDTO : BaseNewsArticleDTO
    {
        public int NewsArticleId { get; set; }
        public int CategoryId { get; set; }
        public int CreatedById { get; set; }
        public int UpdatedById { get; set; }
    }
}
