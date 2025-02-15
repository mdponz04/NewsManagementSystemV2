using Repositories.DTOs.TagDTOs;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<List<GetTagDTO>> GetAllTag();
        Task<GetTagDTO> GetTagById(int id);
        Task<int> CreateTag(PostTagDTO tag);
        Task UpdateTag(PutTagDTO updatedTag);
        Task DeleteTag(int id);
    }
}
