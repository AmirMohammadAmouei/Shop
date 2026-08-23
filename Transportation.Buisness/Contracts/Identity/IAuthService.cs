using Transportation.Buisness._0.Common;
using Transportation.Buisness.Dtos.Auth;

namespace Transportation.Buisness.Contracts.Identity
{
    public interface IAuthService
    {
        Task<Result> Login(LoginRequestDto request);
        Task<Result> LogoutAsync();
    }
}
