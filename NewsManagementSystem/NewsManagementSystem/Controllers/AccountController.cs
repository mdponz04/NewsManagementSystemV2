using Microsoft.AspNetCore.Mvc;

namespace NewsManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
