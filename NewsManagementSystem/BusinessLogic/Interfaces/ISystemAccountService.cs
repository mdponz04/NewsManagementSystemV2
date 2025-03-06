using Data.Enum;
using BusinessLogic.DTOs.SystemAccountDTOs;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface ISystemAccountService
    {
        Task<PaginatedList<GetSystemAccountDTO>> GetUserAccounts(int index, int pageSize, int? idSearch, string? nameSearch, string? emailSearch, EnumRole? role);

        Task<GetSystemAccountDTO> GetUserAccountById(short id);

        Task CreateUserAccount(PostSystemAccountDTO postSystemAccount);

        Task UpdateUserAccountById(PutSystemAccountDTO updatedUserAccount);

        Task DeleteUserAccountById(short id);
        Task<string> Login(LoginDTO loginDTO);
    }
}
