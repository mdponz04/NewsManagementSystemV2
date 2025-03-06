using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.SystemAccountDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.SystemAccounts
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string ADMIN_Role = "0";

        public ProfileModel(ISystemAccountService systemAccountService, IJwtTokenService jwtTokenService)
        {
            _systemAccountService = systemAccountService;
            _jwtTokenService = jwtTokenService;
        }

        public GetSystemAccountDTO SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userRole = _jwtTokenService.GetRole(jwtToken!);

            string curentLoggedInUserId = _jwtTokenService.GetId(jwtToken!);

            if (userRole == null)
            {
                return Unauthorized();
            }

            if (curentLoggedInUserId != id.ToString() && userRole != ADMIN_Role)
            {
                return Forbid();
            }

            SystemAccount = await _systemAccountService.GetUserAccountById(id);

            if (TempData["EditSuccess"] != null)
            {
                ViewData["SuccessMessage"] = TempData["EditSuccess"];
            }

            ViewData["currrentLoggedInUserRole"] = userRole;

            return Page();

        }
    }
}
