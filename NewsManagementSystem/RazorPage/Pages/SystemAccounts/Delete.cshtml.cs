using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.SystemAccountDTOs;

namespace RazorPage.Pages.SystemAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;
        private const string ADMIN_ROLE = "0";

        public DeleteModel(IJwtTokenService jwtTokenService, ISystemAccountService systemAccountService)
        {
            _jwtTokenService = jwtTokenService;
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public GetSystemAccountDTO SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            SystemAccount = await _systemAccountService.GetUserAccountById(id);

            if (SystemAccount == null)  return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            try
            {
                var jwtToken = HttpContext.Session.GetString("jwt_token");
                string userRole = _jwtTokenService.GetRole(jwtToken!);

                if (userRole == null || userRole != ADMIN_ROLE)
                {
                    return Forbid();
                }

                await _systemAccountService.DeleteUserAccountById(id);
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error" + ex.Message;
                throw;
            }
        }
    }
}
