using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Repositories.DTOs.TagDTOs;
using Repositories.Interface;

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
        
    }
}