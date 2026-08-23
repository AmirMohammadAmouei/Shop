using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness.Contracts.Identity;
using Transportation.Buisness.Dtos.Auth;

namespace Transportation.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var response = await _authService.Login(request);

            if (!response.IsSucceeded)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(request);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
