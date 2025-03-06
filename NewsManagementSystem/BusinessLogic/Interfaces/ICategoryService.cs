﻿using BusinessLogic.DTOs.CategoryDTOs;
using Data.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDTO>> GetAllCategories();
        Task<GetCategoryDTO> GetCategoryById(short id);
        Task<PaginatedList<GetCategoryDTO>> GetCategories(int index, int pageSize, int? idSearch, string? nameSearch, string? descriptionSearch, int? parentIdSearch, bool? isActiveSearch);

        Task<CreateCategoryDTO> CreateCategory(CreateCategoryDTO categoryDto);
        Task<UpdateCategoryDTO> UpdateCategory(short categoryId, UpdateCategoryDTO categoryDto);
        Task<bool> DeleteCategory(short categoryId);
    }
}
