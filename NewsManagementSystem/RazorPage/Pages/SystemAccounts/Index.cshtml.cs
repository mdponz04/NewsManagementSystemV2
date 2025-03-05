using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.DTOs.SystemAccountDTOs;
using Data.PaggingItem;
using BusinessLogic.Interfaces;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.SystemAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;
        private const string ADMIN_ROLE = "0";

        public IndexModel(IJwtTokenService jwtTokenService, ISystemAccountService systemAccountService)
        {
            _jwtTokenService = jwtTokenService;
            _systemAccountService = systemAccountService;
        }

        public PaginatedList<GetSystemAccountDTO> SystemAccountsList { get; set; } = default!;


        // GET: Get and Search SystemAccounts
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 3, string? searchNameString = null, string? searchEmailString = null, string? searchRoleString = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userRole = _jwtTokenService.GetRole(jwtToken!);

            if (userRole == null || userRole != ADMIN_ROLE)
            {
                return Forbid();
            }

            EnumRole? roleFilter = null;

            if (!string.IsNullOrEmpty(searchRoleString) && Enum.TryParse(searchRoleString, out EnumRole parsedRole))
            {
                roleFilter = parsedRole;
            }
            // Fetch paginated search categories
            SystemAccountsList = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, searchNameString, searchEmailString, roleFilter);

            return Page();
        }
    }
}