using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.DTOs.SystemAccountDTOs;
using Data.Enum;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        private const string ADMIN_ID = "0";

        public PaginatedList<GetSystemAccountDTO> SystemAccounts { get; set; }
        public IndexModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        // GET: Get and Search SystemAccounts
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchNameString = null, string? searchEmailString = null, string? searchRoleString = null)
        {
            EnumRole? roleFilter = null;
            if (!string.IsNullOrEmpty(searchRoleString) && Enum.TryParse(searchRoleString, out EnumRole parsedRole))
            {
                roleFilter = parsedRole;
            }
            // Fetch paginated search categories
            SystemAccounts = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, searchNameString, searchEmailString, roleFilter);
        }
    }
}
