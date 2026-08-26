using NabeelsPortal.Models;

namespace NabeelsPortal.DTO
{
    public class ViewProducts
    {
        public List<Farmer> Farmers { get; set; }
        public List<Product> Products { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProductType { get; set; }
        public List<string> ProductTypes { get; set; }
    }
}
