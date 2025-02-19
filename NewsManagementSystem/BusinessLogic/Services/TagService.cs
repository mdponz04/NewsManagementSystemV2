using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.DTOs.TagDTOs;
using Repositories.Interface;
using Repositories.PaggingItem;


namespace BusinessLogic.Services
{
    public class TagService : ITagService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;
        public TagService(IUOW unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetTagDTO>> GetAllTag()
        {
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            IEnumerable<Tag> tags = await repository.GetAllAsync();
            return _mapper.Map<List<GetTagDTO>>(tags);
        }

        public async Task<GetTagDTO> GetTagById(int id)
        {
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            Tag? tag = await repository.GetByIdAsync(id);
            return _mapper.Map<GetTagDTO>(tag);
        }

        public async Task<int> CreateTag(PostTagDTO tag)
        {
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            Tag newTag = _mapper.Map<Tag>(tag);
            await repository.InsertAsync(newTag);
            await _unitOfWork.SaveAsync();
            return newTag.TagId;
        }

        public async Task UpdateTag(PutTagDTO updatedTag)
        {
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            Tag? existingTag = await repository.GetByIdAsync(updatedTag.TagId);
            if (existingTag == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Tag not found!");
            }
            // Update properties
            _mapper.Map(updatedTag, existingTag);

            /*existingTag.TagName = updatedTag.TagName;
            existingTag.Note = updatedTag.Note;*/
            // Update other properties as needed
            repository.Update(existingTag);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTag(int id)
        {
            if(await CheckIsTagInUseByTagId(id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Tag is in use!");
            }
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            Tag? tag = await repository.GetByIdAsync(id);

            if (tag != null)
            {
                repository.Delete(tag);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Tag not found!");
            }
        }
        public async Task<List<Tag>> GetListTagByIdEntityType(List<int> ids)
        {
            IGenericRepository<Tag> repository = _unitOfWork.GetRepository<Tag>();
            List<Tag> tags = new List<Tag>();
            foreach(int id in ids)
            {
                Tag? tag = await repository.GetByIdAsync(id);

                if (tag == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Tag not found!");
                }
                else
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }
        private async Task<bool> CheckIsTagInUseByTagId(int id)
        {
            NewsTag? newsTag = await _unitOfWork
                .GetRepository<NewsTag>()
                .Entities
                .Where(NewsTag => NewsTag.TagId == id)
                .FirstOrDefaultAsync();
            return newsTag != null;
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