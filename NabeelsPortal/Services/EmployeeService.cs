using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NabeelsPortal.Data;
using NabeelsPortal.DTO;
using NabeelsPortal.Models;

namespace NabeelsPortal.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly AgriEnergyContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeService(AgriEnergyContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddFarmer(FarmerRegistrationDto farmerDto)
        {
            // Create the user
            var user = new IdentityUser { UserName = farmerDto.Username, Email = farmerDto.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, farmerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user.");
            }

            // Assign the role
            if (!await _roleManager.RoleExistsAsync(farmerDto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(farmerDto.Role));
            }
            await _userManager.AddToRoleAsync(user, farmerDto.Role);

            // Create the farmer profile
            var farmer = new Farmer
            {
                FarmerId = user.Id,
                Name = farmerDto.Name,
                ContactInfo = farmerDto.ContactInfo,
                Address = farmerDto.Address,
                Email = farmerDto.Email
            };

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByFarmer(string farmerId, DateTime? startDate, DateTime? endDate, string productType)
        {
            var query = _context.Products.Where(p => p.FarmerId == farmerId);

            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.Category == productType);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Farmer>> GetFarmers()
        {
            return await _context.Farmers.ToListAsync();
        }

        public async Task<List<string>> GetProductTypes()
        {
            // Assuming you have a DbSet<Product> in your context and Product entity has a property for product type
            var productTypes = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return productTypes;
        }

    }
}
