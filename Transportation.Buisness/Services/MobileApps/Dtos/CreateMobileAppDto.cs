using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Buisness.Services.MobileApps.Dtos
{
    public class CreateMobileAppDto
    {
        [Required(ErrorMessage = "نام اپ الزامی است")]
        public string Title { get; set; }
        [Required(ErrorMessage = "توضیحات اپ موبایل الزامی است")]
        public string Description { get; set; }
        [Required(ErrorMessage = "نسخه اپ الزامی است")]
        public string Version { get; set; }
        [Required(ErrorMessage = "نوع اپ موبایل الزامی است")]
        public string Platform { get; set; }
        public IFormFile File { get; set; }
        public IFormFile Icon { get; set; }
    }
}
