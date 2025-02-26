using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using BusinessLogic.Interfaces;
using Data.DTOs.SystemAccountDTOs;

namespace RazorPage.Pages.SystemAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;

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
