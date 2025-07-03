using InvoiceBillingAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace InvoiceBillingAPI.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task<Customer?> GetByIdAsync(int id);
        public Task<int> CreateAsync(Customer customer);
        public Task<bool> UpdateAsync(Customer customer);
        public Task<bool> DeleteAsync(int id);
    }
}
