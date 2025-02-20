using AutoMapper;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Repositories.DTOs.CategoryDTOs;
using Repositories.Interface;
using Repositories.PaggingItem;
using BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        private const int STAFF = 1;
        private const int LECTURER = 2;
        private const int STARTING_NUMBER = 0;

        public CategoryService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetCategoryDTO>> GetAllCategories()
        {
            IQueryable<Category> query = _unitOfWork.GetRepository<Category>().Entities;
            query = query.OrderBy(c => c.CategoryName);
            IEnumerable<Category> categories = await query.ToListAsync();
            return _mapper.Map<IEnumerable<GetCategoryDTO>>(categories);
        }
        public async Task<GetCategoryDTO> GetCategoryById(short id)
        {
            Category? category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Category not found!");
            }
            GetCategoryDTO responseItem = _mapper.Map<GetCategoryDTO>(category);
            return responseItem;
        }

        // Get list of system tag
        public async Task<PaginatedList<GetCategoryDTO>> GetCategories(int index, int pageSize, int? idSearch, string? nameSearch, string? descriptionSearch, int? parentIdSearch, bool? isActiveSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Category> query = _unitOfWork.GetRepository<Category>().Entities;

            // Search by category id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.CategoryId == idSearch);
            }

            // Search by name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.CategoryName!.Contains(nameSearch));
            }

            // Search by email
            if (!string.IsNullOrWhiteSpace(descriptionSearch))
            {
                query = query.Where(u => u.CategoryDesciption!.Contains(descriptionSearch));
            }

            if (parentIdSearch.HasValue)
            {
                query = query.Where(u => u.ParentCategoryId == parentIdSearch);
            }

            if (isActiveSearch.HasValue)
            {
                query = query.Where(u => u.IsActive == isActiveSearch);
            }

            // Sort by Id
            query = query.OrderBy(u => u.CategoryId);

            PaginatedList<Category> resultQuery = await _unitOfWork.GetRepository<Category>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetCategoryDTO> responseItems = resultQuery.Items.Select(item =>
            {
                //GetCategoryDTO responseItem = _mapper.Map<GetCategoryDTO>(item);
                GetCategoryDTO responseItem = new GetCategoryDTO();
                responseItem.CategoryId = item.CategoryId;
                responseItem.CategoryName = item.CategoryName;
                responseItem.ParentCategoryId = item.CategoryId;
                responseItem.CategoryDescription = item.CategoryDesciption;
                responseItem.IsActive = true;

                return responseItem;
            }).ToList();

            PaginatedList<GetCategoryDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }


        // Create a new category
        public async Task<CreateCategoryDTO> CreateCategory(CreateCategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Category data is required!");
            }

            Category category = _mapper.Map<Category>(categoryDto);

            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CreateCategoryDTO>(category);
        }
        public async Task<UpdateCategoryDTO> UpdateCategory(short categoryId, UpdateCategoryDTO categoryDto)
        {
            if (categoryDto == null || categoryId <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid category data or category ID!");
            }

            // Find the category by id
            Category category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);

            if (category == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Category not found!");
            }

            // Map the updated fields to the category entity
            _mapper.Map(categoryDto, category);

            // Update the category in the database
            _unitOfWork.GetRepository<Category>().Update(category);
            await _unitOfWork.SaveAsync();

            // Return the updated category
            return _mapper.Map<UpdateCategoryDTO>(category);
        }

        // Delete an existing category
        public async Task<bool> DeleteCategory(short categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid category ID!");
            }

            // Find the category by id
            Category category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);

            if (category == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Category not found!");
            }

            // Remove the category from the database
            await _unitOfWork.GetRepository<Category>().DeleteAsync(category);
            await _unitOfWork.SaveAsync();

            return true;
        }


    }
}
