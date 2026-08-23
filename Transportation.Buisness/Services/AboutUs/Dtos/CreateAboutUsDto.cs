using Microsoft.AspNetCore.Http;

namespace Transportation.Buisness.Services.AboutUs.Dtos
{
    public class CreateAboutUsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public IFormFile Logo { get; set; }
    }

}
