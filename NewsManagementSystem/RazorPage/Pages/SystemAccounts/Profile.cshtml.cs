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

        private const string ADMIN_ID = "0";

        public ProfileModel(ISystemAccountService systemAccountService, IJwtTokenService jwtTokenService)
        {
            _systemAccountService = systemAccountService;
            _jwtTokenService = jwtTokenService;
        }

        public GetSystemAccountDTO SystemAccount { get; set; } = default!;

        //public async Task<IActionResult> OnGetAsync(int id)
        //{
        //    GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

        //    string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");

        //    // Retrieve claims using the JWT token service
        //    string userRole = _jwtTokenService.GetRole(jwtTokenFromSession!);
        //    string userId = _jwtTokenService.GetId(jwtTokenFromSession!);

        //    ViewData["UserRole"] = userRole;
        //    ViewData["UserId"] = userId;

        //    if (userId!.Equals(userAccount.AccountId.ToString()) || userId.Equals(ADMIN_ID))
        //    {
        //        return Page();
        //    }
        //    else
        //    {
        //        return RedirectToPage("Home/Index"); // Redirect to Home/Index
        //    }
        //}

        public async Task<IActionResult> OnGetAsync(short id)
        {
            SystemAccount = await _systemAccountService.GetUserAccountById(id);

            if (TempData["EditSuccess"] != null)
            {
                ViewData["SuccessMessage"] = TempData["EditSuccess"];
            }

            return Page();

        }
    }
}
