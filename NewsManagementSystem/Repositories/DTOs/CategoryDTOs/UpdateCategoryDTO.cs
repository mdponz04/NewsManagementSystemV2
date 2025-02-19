using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO : BaseCategoryDTO
    {
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; } = string.Empty;
    }
}
