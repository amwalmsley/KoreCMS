using Kore.CmsApi.Data;
using Kore.CmsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Kore.CmsApi.Services
{
    public class CustomersService(CustomerDbContext customerDbContext) : ICustomersService
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        public async Task<int> GetCustomersCountAsync() => await _customerDbContext.Customers.CountAsync();

        public async Task<List<Customer>> GetAllCustomersAsync(int pageNumber = 0, int pageSize = 0) => await _customerDbContext.Customers
            .Skip(pageNumber * pageSize)
            .Take(pageSize == 0 ? int.MaxValue : pageSize)
            .ToListAsync();

        public async Task<Customer> GetCustomerAsync(int? id) => await _customerDbContext.Customers.FirstAsync(_ => _.Id == id);

        public async Task CreateCustomer(Customer customer)
        {
            _customerDbContext.Add(customer with { Id = null, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now });
            await _customerDbContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer source, Customer dest)
        {
            _customerDbContext.Update(dest with
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                MiddleInitial = source.MiddleInitial,
                Title = source.Title,
                Phone = source.Phone,
                Email = source.Email,
                ModifiedDate = DateTime.Now
            });

            await _customerDbContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _customerDbContext.Remove(customer);
            await _customerDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsCustomerEmailInUseAsync(string? email) => await _customerDbContext.Customers.AnyAsync(_ => _.Email == email);
    }
}
