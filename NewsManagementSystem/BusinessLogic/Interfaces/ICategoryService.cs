using Repositories.DTOs.CategoryDTOs;
using Repositories.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedList<GetCategoryDTO>> GetCategories(int index, int pageSize, int? idSearch, string? nameSearch, string? descriptionSearch, int? parentIdSearch, bool? isActiveSearch);
        Task<IEnumerable<GetCategoryDTO>> GetAllCategories();
        Task<GetCategoryDTO> GetCategoryById(short id);
    }
}
