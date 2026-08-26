using NabeelsPortal.Models;
using System.Threading.Tasks;

namespace NabeelsPortal.Services
{
    public interface IFarmerService
    {
        Task<List<Product>> GetFarmerProducts(string farmerId);
        Task AddProduct(string farmerId, Product product);
    }
}
