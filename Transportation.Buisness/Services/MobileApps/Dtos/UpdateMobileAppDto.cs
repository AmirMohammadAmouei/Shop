using Microsoft.AspNetCore.Http;

namespace Transportation.Buisness.Services.MobileApps.Dtos
{
    public class UpdateMobileAppDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public bool IsActive { get; set; }
        public IFormFile File { get; set; }
        public IFormFile Icon { get; set; }
    }
}
