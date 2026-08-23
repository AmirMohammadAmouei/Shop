using Transportation.Buisness.Services.ProductCategories.Dto;

namespace Transportation.WebUI.Models
{
    public class HomeViewModel
    {
        public List<ProductCategoryListResponseDto> Categories { get; set; } = new();
    }
}
