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
        Task<List<GetTagDTO>> GetAllTag();
        Task<GetTagDTO> GetTagById(int id);
        Task<int> CreateTag(PostTagDTO tag);
        Task UpdateTag(PutTagDTO updatedTag);
        Task DeleteTag(int id);
        Task<List<Tag>> GetListTagByIdEntityType(List<int> ids);
        Task<PaginatedList<GetTagDTO>> GetTags(int index, int pageSize, int? idSearch, string? nameSearch, string? noteSearch);
    }
}
