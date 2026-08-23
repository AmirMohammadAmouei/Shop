using System.ComponentModel.DataAnnotations;

namespace Transportation.Buisness.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string Username { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; }
        public bool RemeberMe { get; set; } = false;
    }
}
