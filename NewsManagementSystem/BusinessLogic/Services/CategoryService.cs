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
                GetCategoryDTO responseItem = _mapper.Map<GetCategoryDTO>(item);

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
    }
}
