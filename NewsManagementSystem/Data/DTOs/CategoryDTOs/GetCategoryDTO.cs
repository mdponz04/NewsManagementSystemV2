using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.CategoryDTOs
{
    public class GetCategoryDTO : BaseCategoryDTO
    {
        public short CategoryId {  get; set; }
        public short ParentCategoryId { get; set; }
    }
}
