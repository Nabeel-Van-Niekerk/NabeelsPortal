using Microsoft.EntityFrameworkCore;
using NabeelsPortal.Data;
using NabeelsPortal.Models;

namespace NabeelsPortal.Services
{
    public class FarmerService: IFarmerService
    {
        private readonly AgriEnergyContext _context;

        public FarmerService(AgriEnergyContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetFarmerProducts(string farmerId)
        {
            return await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }

        public async Task AddProduct(string farmerId, Product product)
        {
            // Check if the farmerId corresponds to an existing farmer
            var existingFarmer = await _context.Farmers.FindAsync(farmerId);
            if (existingFarmer == null)
            {
                // If the farmerId does not correspond to an existing farmer, handle the error appropriately
                throw new ArgumentException("Invalid farmerId.");
            }

            // Create a new product with the provided details
            var newProduct = new Product
            {
                FarmerId = farmerId,
                ProductName = product.ProductName,
                Category = product.Category,
                ProductionDate = product.ProductionDate,
            };

            // Add the new product to the context and save changes
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
        }

    }
}
