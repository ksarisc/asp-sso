using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspSso.Controllers
{
    [Authorize]
    public class SecuredController : Controller
    {
        private readonly ILogger<SecuredController> logger;

        public SecuredController(ILogger<SecuredController> securedLogger)
        {
            logger = securedLogger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
