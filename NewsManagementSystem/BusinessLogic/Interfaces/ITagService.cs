using Data.Entities;
using BusinessLogic.DTOs.TagDTOs;
using Data.PaggingItem;
namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<List<GetTagDTO>> GetAllTag();
        Task<GetTagDTO> GetTagById(int id);
        Task CreateTag(PostTagDTO tag);
        Task UpdateTag(PutTagDTO updatedTag);
        Task DeleteTag(int id);
        Task<List<Tag>> GetListTagByIdEntityType(List<int> ids);
        Task<List<GetTagDTO>> GetListTag(List<int> tagIds);
        Task<PaginatedList<GetTagDTO>> GetTags(int index, int pageSize, int? idSearch, string? nameSearch, string? noteSearch);
    }
}
