using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NabeelsPortal.DTO;
using NabeelsPortal.Services;
using System.Security.Claims;

namespace NabeelsPortal.Controllers
{
    [Authorize(Roles = "Employee")]
    [Route("[controller]")]
    public class EmployeeController: Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ViewFarmers")]
        public async Task<IActionResult> GetFarmers()
        {
            var farmers = await _employeeService.GetFarmers();
            return View("ViewFarmers", farmers);
        }

        [HttpGet("ViewProducts")]
        public IActionResult GetProducts()
        {
            // Display form for filtering products
            return View();
        }


        [HttpGet("Products")]
        public async Task<IActionResult> ViewProducts(string farmerId, DateTime? startDate, DateTime? endDate, string productType)
        {
            var farmers = await _employeeService.GetFarmers();
            var productTypes = await _employeeService.GetProductTypes();

            // Get filtered products for the specified farmer
            var products = await _employeeService.GetProductsByFarmer(farmerId, startDate, endDate, productType);

            var viewModel = new ViewProducts
            {
                Farmers = farmers,
                Products = products,
                StartDate = startDate,
                EndDate = endDate,
                ProductType = productType,
                ProductTypes = productTypes
            };

            ViewBag.FarmerId = farmerId;

            return View("ViewAndFilterProducts", viewModel);
        }



        [HttpGet("AddFarmer")]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost("AddFarmer")]
        public async Task<IActionResult> AddFarmer(FarmerRegistrationDto farmerDto)
        {
            try
            {
                var userRoles = ((ClaimsIdentity)User.Identity).Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);
                foreach (var role in userRoles)
                {
                    Console.WriteLine($"User has role: {role}");
                }
                await _employeeService.AddFarmer(farmerDto);
                TempData["SuccessMessage"] = "Farmer Added Successfully";
                return RedirectToAction("ViewFarmers");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
