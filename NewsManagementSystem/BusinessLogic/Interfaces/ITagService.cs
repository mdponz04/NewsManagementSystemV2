using Repositories.DTOs.TagDTOs;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<List<GetTagDTO>> GetAllTag();
        Task<GetTagDTO> GetTagById(short id);
        Task CreateTag(PostTagDTO tag);
        Task UpdateTag(PutTagDTO updatedTag);
        Task DeleteTag(short id);
    }
}
