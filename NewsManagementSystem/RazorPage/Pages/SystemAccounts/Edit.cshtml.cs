using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.SystemAccountDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.SystemAccounts
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private const string ADMIN_Role = "0";

        public EditModel(ISystemAccountService systemAccountService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _systemAccountService = systemAccountService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public PutSystemAccountDTO SystemAccountDto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
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

            short formattedId = (short)id;

            var getAccountDto = await _systemAccountService.GetUserAccountById(formattedId);
            if (getAccountDto == null)
            {
                return NotFound();
            }

            SystemAccountDto = _mapper.Map<PutSystemAccountDTO>(getAccountDto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Partial("_EditSystemAccountPartial", SystemAccountDto);
            }

           return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] PutSystemAccountDTO systemAccountDto)
        {
            if (!ModelState.IsValid || systemAccountDto == null)
            {
                return new JsonResult(new { success = false, error = "Validation failed" });
            }

            try
            {
                await _systemAccountService.UpdateUserAccountById(systemAccountDto);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }
    }
}