using Kore.CmsApi.Models;

namespace Kore.CmsApi.Services
{
    public interface ICustomersService
    {
        Task CreateCustomer(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync(int pageNumber = 0, int pageSize = 0);
        Task<Customer> GetCustomerAsync(int? id);
        Task<int> GetCustomersCountAsync();
        Task<bool> IsCustomerEmailInUseAsync(string? email);
        Task UpdateCustomerAsync(Customer source, Customer dest);
    }
}