using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Buisness.Services.Customers.Dtos
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage ="نام مشتری اجباری است")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لوگو مشتری اجباری است")]
        public IFormFile LogoPath { get; set; }
    }
}
