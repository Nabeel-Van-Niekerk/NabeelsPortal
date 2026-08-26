using NabeelsPortal.DTO;
using NabeelsPortal.Models;

namespace NabeelsPortal.Services
{
    public interface IEmployeeService
    {
        Task AddFarmer(FarmerRegistrationDto farmerDto);
        Task<List<Product>> GetProductsByFarmer(string farmerId, DateTime? startDate, DateTime? endDate, string productType);

        Task<List<Farmer>> GetFarmers();

        Task<List<string>> GetProductTypes();
    }
}
