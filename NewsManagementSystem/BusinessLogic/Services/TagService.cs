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
            var repository = _unitOfWork.GetRepository<Tag>();
            var tags = (await repository.GetAllAsync());
            return _mapper.Map<List<GetTagDTO>>(tags);
        }

        public async Task<GetTagDTO> GetTagById(short id)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var tag = await repository.GetByIdAsync(id);
            return _mapper.Map<GetTagDTO>(tag);
        }

        public async Task CreateTag(PostTagDTO tag)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            await repository.InsertAsync(_mapper.Map<Tag>(tag));
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateTag(PutTagDTO updatedTag)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var existingTag = await repository.GetByIdAsync(updatedTag.TagId);
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

        public async Task DeleteTag(short id)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var tag = await repository.GetByIdAsync(id);
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