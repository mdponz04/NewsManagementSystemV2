using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.Enum;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.Interface;
using Repositories.PaggingItem;
using Repositories.DTOs.TagDTOs;

namespace BusinessLogic.Services
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        private const int STAFF = 1;
        private const int LECTURER = 2;
        private const int STARTING_NUMBER = 0;

        public TagService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Get list of system tag
        public async Task<PaginatedList<GetTagDTO>> GetTags(int index, int pageSize, int? idSearch, string? nameSearch, string? noteSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Tag> query = _unitOfWork.GetRepository<Tag>().Entities;

            // Search by tag id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.TagId == idSearch);
            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.TagName!.Contains(nameSearch));
            }

            // Search by email
            if (!string.IsNullOrWhiteSpace(noteSearch))
            {
                query = query.Where(u => u.Note!.Equals(noteSearch));
            }


            // Sort by Id
            query = query.OrderBy(u => u.TagId);

            PaginatedList<Tag> resultQuery = await _unitOfWork.GetRepository<Tag>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetTagDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetTagDTO responseItem = _mapper.Map<GetTagDTO>(item);

                return responseItem;
            }).ToList();

            PaginatedList<GetTagDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }


    }
}
