using Transportation.Buisness.Services.AboutUs.Dtos;
using Transportation.Buisness.Services.ProductCategories.Dto;

namespace Transportation.WebUI.Models
{
    public class HomeViewModel
    {
        public AboutUsResponseDto AboutUs { get; set; }
        public List<ProductCategoryListResponseDto> Categories { get; set; } = new();
    }
}
