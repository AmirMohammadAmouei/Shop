using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common;
using Transportation.Buisness.Services.Customers;
using Transportation.Buisness.Services.Customers.Dtos;
using static System.Net.WebRequestMethods;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(CustomerListRequestDto request)
        {
            var response = await _customerService.List(request);

            if (response.IsSucceeded)
                return View(response.Data);

            return View(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(long id)
        {
            var response = await _customerService.GetDetails(id);

            if (response.IsSucceeded)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateCustomerDto request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault());

            var response = await _customerService.Create(request);

            if (response.IsSucceeded)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateCustomerDto request)
        {
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault());


            var response = await _customerService.Update(request);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _customerService.Delete(id);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }
    }
}
