using Data.Enum;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface ISystemAccountService
    {
        Task<PaginatedList<GetSystemAccountDTO>> GetUserAccounts(int index, int pageSize, int? idSearch, string? nameSearch, string? emailSearch, EnumRole? role);
        Task<PaginatedList<GetSystemAccountDTO>> GetUserAccounts(object index, object pageSize, object idSearch, object nameSearch, object emailSearch, object role);
    }
}
