using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.SystemAccountDTOs;
using BusinessLogic.Services;

namespace RazorPage.Pages.SystemAccounts
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;
        private const string ADMIN_ROLE = "0";

        public CreateModel(ISystemAccountService systemAccountService, IJwtTokenService jwtTokenService)
        {
            _systemAccountService = systemAccountService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public PostSystemAccountDTO SystemAccount { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userRole = _jwtTokenService.GetRole(jwtToken!);

            if (userRole == null || userRole != ADMIN_ROLE)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _systemAccountService.CreateUserAccount(SystemAccount);
            return Page();
        }
    }
}