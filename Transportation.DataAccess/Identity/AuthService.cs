using Microsoft.AspNetCore.Identity;
using Transportation.Buisness._0.Common;
using Transportation.Buisness.Contracts.Identity;
using Transportation.Buisness.Dtos.Auth;
using Transportation.Entities.Entities;

namespace Transportation.DataAccess.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result> Login(LoginRequestDto request)
        {
            if (request == null)
                return Result.Failed("داده ارسالی نامعتبر است");

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
                return Result.Failed("نام کاربری نامعتبر است");

            var login = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: request.RemeberMe, lockoutOnFailure: false);

            if (!login.Succeeded)
                return Result.Failed("نام کاربری یا رمز عبور نامعتبر است");

            return Result.Success();
        }

        public async Task<Result> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Result.Success();
        }
    }
}
