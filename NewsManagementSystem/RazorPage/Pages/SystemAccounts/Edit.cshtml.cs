using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.SystemAccountDTOs;

namespace RazorPage.Pages.SystemAccounts
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public EditModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public GetSystemAccountDTO SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            SystemAccount = await _systemAccountService.GetUserAccountById(id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(PutSystemAccountDTO systemAccount)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _systemAccountService.UpdateUserAccountById(systemAccount);
            return new EmptyResult();
        }
    }
}
