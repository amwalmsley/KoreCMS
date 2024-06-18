using Kore.CmsApi.Models;
using Kore.CmsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kore.CmsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController(ILogger<CustomersController> logger, ICustomersService customersService) : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger = logger;
        private readonly ICustomersService _customersService = customersService;

        [HttpGet()]
        public async Task<ActionResult<PagedResult<Customer>>> GetCustomersAsync([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 0)
        {
            try
            {
                var result = new PagedResult<Customer>
                {
                    TotalCount = await _customersService.GetCustomersCountAsync(),
                    Items = await _customersService.GetAllCustomersAsync(pageNumber, pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerAsync(int id)
        {
            try
            {
                return Ok(await _customersService.GetCustomerAsync(id));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ServiceFilter<ValidationFilterAttribute>]
        public async Task<IActionResult> CreateCustomerAsync(Customer customer)
        {
            try
            {
                if (await _customersService.IsCustomerEmailInUseAsync(customer.Email))
                {
                    ModelState.AddModelError("email", "This email address is already in use");
                    return UnprocessableEntity();
                }

                await _customersService.CreateCustomer(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [ServiceFilter<ValidationFilterAttribute>]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                var customerToUpdate = await _customersService.GetCustomerAsync(customer.Id);
                if (customerToUpdate == null)
                {
                    return NotFound();
                }

                if (customer.Email != customerToUpdate.Email && await _customersService.IsCustomerEmailInUseAsync(customer.Email))
                {
                    ModelState.AddModelError("email", "This email address is already in use");
                    return UnprocessableEntity();
                }

                await _customersService.UpdateCustomerAsync(customer, customerToUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var customer = await _customersService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customersService.DeleteCustomerAsync(customer);
            return Ok();
        }
    }
}
