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

namespace BusinessLogic.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        private const int _staff = 1;
        private const int _lecturer = 2;

        public SystemAccountService(IMapper mapper, IUOW unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        // Get list of system user
        public async Task<PaginatedList<GetSystemAccountDTO>> GetUserAccounts(int index, int pageSize, int? idSearch, string? nameSearch, string? emailSearch, EnumRole? role)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Vui lòng nhập số trang hợp lệ");
            }

            IQueryable<SystemAccount> query = _unitOfWork.GetRepository<SystemAccount>().Entities;

            // Search by user id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.AccountId == idSearch);
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
                case (EnumRole?)_staff:
                    query = query.Where(u => u.AccountRole == _staff);
                    break;
                case (EnumRole?)_lecturer:
                    query = query.Where(u => u.AccountRole == _lecturer);
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
    }
}
