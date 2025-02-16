using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.PaggingItem;
using BusinessLogic.Interfaces;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;

namespace NewsManagementSystem.Controllers
{
    public class SystemAccountsController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        // GET: SystemAccounts
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {

                // Fetch paginated user accounts
                PaginatedList<GetSystemAccountDTO> userAccounts = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, null, null, null);

                // Pass data to the view
                return View(userAccounts);
        }

        // GET: SystemAccounts/Profile/{id}
        public async Task<IActionResult> Profile(short id)
        {
            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            return View(userAccount);
        }

        // GET: SystemAccounts/Create
        public IActionResult Create()
        {
            return PartialView("_Create", new PostSystemAccountDTO());
        }

        // POST: SystemAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: SystemAccounts/Edit/5
        public async Task<IActionResult> Edit(short id)
        {
            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            return PartialView("_Edit", userAccount);
        }

        // POST: SystemAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Delete(short id)
        {
            GetSystemAccountDTO userAccount = await _systemAccountService.GetUserAccountById(id);

            return View(userAccount);
        }

        // POST: SystemAccounts/Delete/5
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
        public async Task<IActionResult> Search(int pageNumber = 1, int pageSize = 3, string? searchString = null)
        {

            // Fetch paginated user accounts
            PaginatedList<GetSystemAccountDTO> userAccountsSearch = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, searchString, null, null);

            if (userAccountsSearch == null)
            {
                userAccountsSearch = await _systemAccountService.GetUserAccounts(pageNumber, pageSize, null, null, searchString, null);
            }

            // Pass data to the view
            return View("Index", userAccountsSearch);
        }


    }
}
