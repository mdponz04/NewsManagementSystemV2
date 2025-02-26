using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.DTOs.SystemAccountDTOs;

namespace RazorPage.Pages.SystemAccounts
{
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public CreateModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public PostSystemAccountDTO SystemAccount { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _systemAccountService.CreateUserAccount(SystemAccount);
            return new EmptyResult();
        }
    }
}