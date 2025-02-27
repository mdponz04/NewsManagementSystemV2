using Data.Entities;
using Data.DTOs.TagDTOs;
using Data.PaggingItem;
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
