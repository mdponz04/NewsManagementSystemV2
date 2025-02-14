using Repositories.DTOs.TagDTOs;
using Repositories.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<PaginatedList<GetTagDTO>> GetTags(int index, int pageSize, int? idSearch, string? nameSearch, string? noteSearch);
    }
}
