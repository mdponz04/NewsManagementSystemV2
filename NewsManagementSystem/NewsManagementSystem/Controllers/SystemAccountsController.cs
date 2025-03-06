using Microsoft.AspNetCore.Mvc;
using BusinessLogic.DTOs.SystemAccountDTOs;
using Data.PaggingItem;
using BusinessLogic.Interfaces;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountsController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string ADMIN_ID = "0";
        public SystemAccountsController(ISystemAccountService systemAccountService, IJwtTokenService jwtTokenService)
        {
            _systemAccountService = systemAccountService;
            _jwtTokenService = jwtTokenService;
        }

        // GET: SystemAccounts
        [Authorize(Roles = "0")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            // Fetch paginated user accounts
            PaginatedList<GetSystemAccountDTO> userAccounts = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, null, null, null);

            // Pass data to the view
            return View(userAccounts);
        }

        [Authorize(Roles = "0, 1, 2")]
        // GET: SystemAccounts/Profile/{id}
        public async Task<IActionResult> Profile(short id)
        {
            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            string? jwtTokenFromSession = HttpContext.Session.GetString("jwt_token");

            // Retrieve claims using the JWT token service
            string userRole = _jwtTokenService.GetRole(jwtTokenFromSession!);
            string userId = _jwtTokenService.GetId(jwtTokenFromSession!);

            ViewData["UserRole"] = userRole;
            ViewData["UserId"] = userId;

            if (userId!.Equals(userAccount.AccountId.ToString()) || userId.Equals(ADMIN_ID))
            {
                return View(userAccount);
            } else
            {
                return RedirectToAction("Index", "Home"); // Redirect to Home/Index
            }
        }

        // GET: SystemAccounts/Create
        [Authorize(Roles = "0")]
        public IActionResult Create()
        {
            return PartialView("_Create", new PostSystemAccountDTO());
        }

        // POST: SystemAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0")]
        public async Task<IActionResult> Create(PostSystemAccountDTO systemAccount)
        {
            try
            {
                await _systemAccountService.CreateUserAccount(systemAccount);
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: SystemAccounts/Edit/{id}
        [Authorize]
        public async Task<IActionResult> Edit(short id)
        {

            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            return PartialView("_Edit", userAccount);
        }


        // POST: SystemAccounts/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(PutSystemAccountDTO systemAccount)
        {
            try
            {
                await _systemAccountService.UpdateUserAccountById(systemAccount);
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Profile", new { id = systemAccount.AccountId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: SystemAccounts/Delete/5
        [Authorize(Roles = "0")]
        public async Task<IActionResult> Delete(short id)
        {
            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            return View(userAccount);
        }

        // POST: SystemAccounts/Delete/5
        [Authorize(Roles = "0")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            try
            {
                await _systemAccountService.DeleteUserAccountById(id);
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error" + ex.Message;
                throw;
            }

        }


        // GET: Search SystemAccounts
        [Authorize(Roles = "0")]
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchNameString = null, string? searchEmailString = null, string? searchRoleString = null)
        {
            EnumRole? roleFilter = null;
            if (!string.IsNullOrEmpty(searchRoleString) && Enum.TryParse(searchRoleString, out EnumRole parsedRole))
            {
                roleFilter = parsedRole;
            }

            // Fetch paginated user accounts
            PaginatedList<GetSystemAccountDTO> userAccountsSearch = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, searchNameString, searchEmailString, roleFilter);

            // Pass data to the view
            return View("Index", userAccountsSearch);
        }


    }
}
