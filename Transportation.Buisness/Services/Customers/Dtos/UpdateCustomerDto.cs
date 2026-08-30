using Microsoft.AspNetCore.Http;

namespace Transportation.Buisness.Services.Customers.Dtos
{
    public class UpdateCustomerDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? LogoPath { get; set; }
    }
}
