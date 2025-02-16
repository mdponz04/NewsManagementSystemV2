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

        // Get list of newsarticle
        public async Task<PaginatedList<GetNewsArticleDTO>> GetNewsArticles(int index, int pageSize, string? idSearch, string? titleSearch, string? headlineSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<NewsArticle> query = _unitOfWork.GetRepository<NewsArticle>().Entities;

            // Search by id
            if (!string.IsNullOrWhiteSpace(idSearch))
            {
                query = query.Where(u => u.NewsArticleId!.Contains(idSearch));
            }

            // Search by title
            if (!string.IsNullOrWhiteSpace(titleSearch))
            {
                query = query.Where(u => u.NewsTitle!.Contains(titleSearch));
            }

            // Search by headline
            if (!string.IsNullOrWhiteSpace(headlineSearch))
            {
                query = query.Where(u => u.Headline!.Equals(headlineSearch));
            }


            // Sort by Id
            query = query.OrderBy(u => u.NewsArticleId);

            PaginatedList<NewsArticle> resultQuery = await _unitOfWork.GetRepository<NewsArticle>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetNewsArticleDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetNewsArticleDTO responseItem = _mapper.Map<GetNewsArticleDTO>(item);

            //    responseItem.RoleName = item.AccountRole == STAFF ? "Staff" : "Lecturer";

                return responseItem;
            }).ToList();

            PaginatedList<GetNewsArticleDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }
        
    }
}
