using System.ComponentModel.DataAnnotations;

namespace Transportation.Buisness.Services.ProductCategories.Dto
{
    public class CreateProductCategoryDto
    {
        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        public string Name { get; set; }
    }
}
