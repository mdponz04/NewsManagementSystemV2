using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.Enum;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Repositories.DTOs.NewsArticleDTOs;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.Interface;
using Repositories.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        

        public NewsArticleService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        /*
        // Get list of newsarticle
        public async Task<PaginatedList<GetNewsArticleDTO>> NewsArticles(int index, int pageSize, int? idSearch, string? titleSearch, string? headlineSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<NewsArticle> query = _unitOfWork.GetRepository<NewsArticle>().Entities;

            // Search by id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.NewsArticleId == idSearch);
            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.AccountName!.Contains(nameSearch));
            }

            // Search by email
            if (!string.IsNullOrWhiteSpace(emailSearch))
            {
                query = query.Where(u => u.AccountEmail!.Equals(emailSearch));
            }

            // Search by role
            switch (role)
            {
                case (EnumRole?)STAFF:
                    query = query.Where(u => u.AccountRole == STAFF);
                    break;
                case (EnumRole?)LECTURER:
                    query = query.Where(u => u.AccountRole == LECTURER);
                    break;
                default:
                    break;
            }

            // Sort by Id
            query = query.OrderBy(u => u.AccountId);

            PaginatedList<SystemAccount> resultQuery = await _unitOfWork.GetRepository<SystemAccount>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetSystemAccountDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetSystemAccountDTO responseItem = _mapper.Map<GetSystemAccountDTO>(item);

                responseItem.RoleName = item.AccountRole == STAFF ? "Staff" : "Lecturer";

                return responseItem;
            }).ToList();

            PaginatedList<GetSystemAccountDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }
        */
    }
}
