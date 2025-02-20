using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Data.DTOs.NewsArticleDTOs;
using Data.Interface;
using Data.PaggingItem;


namespace BusinessLogic.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;

        public NewsArticleService(IUOW unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetNewsArticleDTO>> GetAllNewsArticle()
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            IEnumerable<NewsArticle> newsArticles = await repository.GetAllAsync();
            List<NewsArticle> newsArticleList = newsArticles.ToList();
            return _mapper.Map<List<GetNewsArticleDTO>>(newsArticleList);
        }

        public async Task<GetNewsArticleDTO> GetNewsArticleById(string id)
        {
            IQueryable<NewsArticle> query = _unitOfWork.GetRepository<NewsArticle>().Entities;
            NewsArticle? newsArticle = await query.FirstOrDefaultAsync(na => na.NewsArticleId == id);
            List<NewsTag> newsTagList = await _unitOfWork.GetRepository<NewsTag>().Entities
                .Where(nt => nt.NewsArticleId == id)
                .ToListAsync();
            
            if (newsArticle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }
            newsArticle.NewsTags = newsTagList;

            GetNewsArticleDTO showNewsArticle = _mapper.Map<GetNewsArticleDTO>(newsArticle);
            showNewsArticle.CreatedByName = await GetCreatedNameByArticleId(id);
            showNewsArticle.UpdatedByName = await GetUpdatedNameByArticleId(id);
            return showNewsArticle;
        }

        public async Task<string> CreateNewsArticle(PostNewsArticleDTO newsArticle)
        {
            ValidateNewsArticle(_mapper.Map<NewsArticle>(newsArticle));
            //Create new news article
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle newNewsArticle = _mapper.Map<NewsArticle>(newsArticle);
            newNewsArticle.NewsArticleId = await GenerateNewIdAsync();
            newNewsArticle.CreatedDate = DateTime.Now;
            newNewsArticle.ModifiedDate = DateTime.Now;
            
            newNewsArticle.NewsStatus = true;
            await repository.InsertAsync(newNewsArticle);


            //Create news tag that is show relationship between news article and tag
            foreach (int selectedTagId in newsArticle.SelectedTags)
            {
                NewsTag newsTag = new();
                newsTag.NewsArticleId = newNewsArticle.NewsArticleId;
                newsTag.TagId = selectedTagId;

                await _unitOfWork.GetRepository<NewsTag>().InsertAsync(newsTag);
            }
            
            await _unitOfWork.SaveAsync();
            return newNewsArticle.NewsArticleId;
        }
        private void ValidateNewsArticle(NewsArticle newsArticle)
        {
            if (string.IsNullOrWhiteSpace(newsArticle.NewsTitle))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News title is required.");
            }
            if (string.IsNullOrWhiteSpace(newsArticle.Headline))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Headline is required.");
            }
            if (string.IsNullOrWhiteSpace(newsArticle.NewsContent))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News content is required.");
            }
            if (string.IsNullOrWhiteSpace(newsArticle.NewsSource))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News source is required.");
            }
        }
        public async Task UpdateNewsArticle(PutNewsArticleDTO updatedNewsArticle)
        {
            ValidateNewsArticle(_mapper.Map<NewsArticle>(updatedNewsArticle));

            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? existingNewsArticle = await repository
                .GetByIdAsync(updatedNewsArticle.NewsArticleId);

            if (existingNewsArticle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }

            IGenericRepository<NewsTag> newsTagRepo = _unitOfWork.GetRepository<NewsTag>();
            // Remove existing tags
            List<NewsTag> existingNewsTag = await _unitOfWork
                .GetRepository<NewsTag>()
                .Entities
                .Where(nt => nt.NewsArticleId == existingNewsArticle.NewsArticleId)
                .ToListAsync();

            foreach (NewsTag newsTag in existingNewsTag)
            {
                if (newsTag == null) continue;
                existingNewsArticle.NewsTags?.Remove(newsTag);
                newsTagRepo.Delete(newsTag);
            }

            // Update properties
            _mapper.Map(updatedNewsArticle, existingNewsArticle);
            //Add new tags to NewsTag table
            foreach (int selectedTagId in updatedNewsArticle.SelectedTags)
            {
                NewsTag newsTag = new();
                newsTag.NewsArticleId = existingNewsArticle.NewsArticleId;
                newsTag.TagId = selectedTagId;

                await _unitOfWork.GetRepository<NewsTag>().InsertAsync(newsTag);
            }

            repository.Update(existingNewsArticle);
            await _unitOfWork.SaveAsync();
        }
        
        public async Task DeleteNewsArticle(string id)
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? newsArticle = await repository.GetByIdAsync(id);

            if (newsArticle != null)
            {
                List<NewsTag> newsTagsToRemove = _unitOfWork.GetRepository<NewsTag>().Entities
                    .Where(tag => tag.NewsArticleId == id).ToList();
                foreach(var newsTag in newsTagsToRemove)
                {
                    _unitOfWork.GetRepository<NewsTag>().Delete(newsTag);
                }
                repository.Delete(newsArticle);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }
        }
        public async Task InactiveNewsArticle(string id)
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? newsArticle = await repository.GetByIdAsync(id);
            if (newsArticle != null)
            {
                newsArticle.NewsStatus = false;
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }
        }
        //na = news article
        public async Task<List<GetNewsArticleDTO>> GetActiveNewsArticle()
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            List<NewsArticle> newsArticles = (await repository.GetAllAsync())
                .Where(na => na.NewsStatus == true)
                .ToList();
            return _mapper.Map<List<GetNewsArticleDTO>>(newsArticles);
        }
        
        public async Task<List<GetNewsArticleDTO>> GetNewsArticleAccordingToCreateById(short createById)
        {
            // Get all news articles created by the user
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            List<NewsArticle> newsArticles = (await repository.GetAllAsync())
                .Where(na => na.CreatedById == createById)
                .ToList();

            List<GetNewsArticleDTO> showNewsArticles = _mapper.Map<List<GetNewsArticleDTO>>(newsArticles);
            // Get the name of the user who created the article
            foreach (GetNewsArticleDTO showNewsArticle in showNewsArticles)
            {
                showNewsArticle.CreatedByName = await GetCreatedNameByArticleId(showNewsArticle.NewsArticleId);
            }

            return showNewsArticles;
        }
        private async Task<string> GetCreatedNameByArticleId(string articleIds)
        {
            NewsArticle? newsArticle = await _unitOfWork.GetRepository<NewsArticle>()
                .Entities
                .Where(na => na.NewsArticleId == articleIds)
                .FirstOrDefaultAsync();

            string? createdName = await _unitOfWork.GetRepository<SystemAccount>()
                .Entities
                .Where(sa => sa.AccountId == newsArticle.CreatedById)
                .Select(sa => sa.AccountName)
                .FirstOrDefaultAsync();

            return createdName;
        }
        private async Task<string> GetUpdatedNameByArticleId(string articleIds)
        {
            NewsArticle? newsArticle = await _unitOfWork.GetRepository<NewsArticle>()
                .Entities
                .Where(na => na.NewsArticleId == articleIds)
                .FirstOrDefaultAsync();

            string? updatedName = await _unitOfWork.GetRepository<SystemAccount>()
                .Entities
                .Where(sa => sa.AccountId == newsArticle.UpdatedById)
                .Select(sa => sa.AccountName)
                .FirstOrDefaultAsync();

            return updatedName;
        }
        private async Task<string> GenerateNewIdAsync()
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();

            // Get all articles and find the highest ID
            IEnumerable<NewsArticle> allArticles = await repository.GetAllAsync();
            NewsArticle? lastArticle = allArticles.OrderByDescending(n => int.Parse(n.NewsArticleId)).FirstOrDefault();

            // If no articles exist, start from 1
            int newId = (lastArticle == null) ? 1 : int.Parse(lastArticle.NewsArticleId) + 1;

            return newId.ToString();
        }
        public async Task<List<GetNewsArticleDTO>> GetNewsArticleBySearchString(string search)
        {
            List<NewsArticle> newsArticleList = await _unitOfWork.GetRepository<NewsArticle>()
                .Entities
                .Where(na => na.NewsTitle.Contains(search) && na.NewsStatus == true)
                .ToListAsync();

            return _mapper.Map<List<GetNewsArticleDTO>>(newsArticleList);
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