using Microsoft.AspNetCore.Mvc.RazorPages;
using Data.Entities;
using Data.DTOs.SystemAccountDTOs;
using Data.PaggingItem;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.SystemAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;

        public IndexModel(IJwtTokenService jwtTokenService, ISystemAccountService systemAccountService)
        {
            _jwtTokenService = jwtTokenService;
            _systemAccountService = systemAccountService;
        }

        public PaginatedList<GetSystemAccountDTO> SystemAccounts { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 3)
        {
            SystemAccounts = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, null, null, null);

            return Page();
        }
    }
}
