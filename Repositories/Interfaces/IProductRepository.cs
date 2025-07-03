using InvoiceBillingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace InvoiceBillingAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> GetByIdAsync(int id);
        public Task<int> CreateAsync(Product product);
        public Task<bool> UpdateAsync(Product product);
        public Task<bool> DeleteAsync(int id);
    }
}
