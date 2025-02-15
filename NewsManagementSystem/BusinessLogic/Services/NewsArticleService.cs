using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Repositories.DTOs.NewsArticleDTOs;
using Repositories.Interface;

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
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? newsArticle = await repository.GetByIdAsync(id, na => na.Tags);
            if (newsArticle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }
            Console.WriteLine(newsArticle);
            return _mapper.Map<GetNewsArticleDTO>(newsArticle);
        }

        public async Task<string> CreateNewsArticle(PostNewsArticleDTO newsArticle)
        {
            // Validate news article fields
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

            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle newNewsArticle = _mapper.Map<NewsArticle>(newsArticle);
            // Generate unique id by iterating until a unique id is found(increment)
            newNewsArticle.NewsArticleId = await GenerateNewIdAsync();
            newNewsArticle.CreatedDate = DateTime.Now;
            newNewsArticle.ModifiedDate = DateTime.Now;
            newNewsArticle.NewsStatus = true;

            await repository.InsertAsync(newNewsArticle);
            await _unitOfWork.SaveAsync();
            return newNewsArticle.NewsArticleId;
        }

        public async Task UpdateNewsArticle(PutNewsArticleDTO updatedNewsArticle)
        {
            // Validate news article fields
            if (string.IsNullOrWhiteSpace(updatedNewsArticle.NewsTitle))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News title is required.");
            }
            if (string.IsNullOrWhiteSpace(updatedNewsArticle.Headline))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Headline is required.");
            }
            if (string.IsNullOrWhiteSpace(updatedNewsArticle.NewsContent))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News content is required.");
            }
            if (string.IsNullOrWhiteSpace(updatedNewsArticle.NewsSource))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "News source is required.");
            }

            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? existingNewsArticle = await repository
                .GetByIdAsync(updatedNewsArticle.NewsArticleId);
            if (existingNewsArticle == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "News article not found!");
            }
            // Update properties
            _mapper.Map(updatedNewsArticle, existingNewsArticle);
            
            /*existingNewsArticle.NewsTitle = updatedNewsArticle.NewsTitle;
            existingNewsArticle.Headline = updatedNewsArticle.Headline;
            existingNewsArticle.NewsContent = updatedNewsArticle.NewsContent;
            existingNewsArticle.NewsSource = updatedNewsArticle.NewsSource;
            existingNewsArticle.ModifiedDate = DateTime.Now;*/

            repository.Update(existingNewsArticle);
            await _unitOfWork.SaveAsync();
        }
        
        public async Task DeleteNewsArticle(string id)
        {
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            NewsArticle? newsArticle = await repository.GetByIdAsync(id);
            if (newsArticle != null)
            {
                newsArticle.NewsStatus = false;
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
            IGenericRepository<NewsArticle> repository = _unitOfWork.GetRepository<NewsArticle>();
            List<NewsArticle> newsArticles = (await repository.GetAllAsync())
                .Where(na => na.CreatedById == createById)
                .ToList();
            return _mapper.Map<List<GetNewsArticleDTO>>(newsArticles);
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
    }
}