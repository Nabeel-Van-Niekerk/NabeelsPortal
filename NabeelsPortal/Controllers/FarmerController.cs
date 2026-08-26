using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NabeelsPortal.Models;
using NabeelsPortal.Services;
using System.Security.Claims;

namespace NabeelsPortal.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FarmerController: Controller
    {
        public readonly IFarmerService _farmerService;

        public FarmerController(IFarmerService farmerService)
        {
            _farmerService = farmerService;
        }

        private string GetCurrentUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("ViewProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var farmerId = GetCurrentUserId();
            var products = await _farmerService.GetFarmerProducts(farmerId);
            return View("ViewProducts", products);
        }

        [HttpGet("AddProduct")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // Only farmers should be able to add products
            if (User.IsInRole("Farmer"))
            {
                var farmerId = GetCurrentUserId();
                await _farmerService.AddProduct(farmerId, product);
                return RedirectToAction("ViewProducts");
            }
            else
            {
                return Forbid(); // Return 403 Forbidden if the user is not a farmer
            }
        }

    }
}
