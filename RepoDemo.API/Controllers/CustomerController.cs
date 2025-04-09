using RepoDemo.Data.Entities;
using RepoDemo.Service;
using Microsoft.AspNetCore.Mvc;

namespace RepoDemo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return result.IsFailure && result.Errors.Contains("Customer not found.")
                ? NotFound(result.Errors)
                : StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _customerService.AddCustomerAsync(customer);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest("Customer ID mismatch.");
            }

            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer.IsFailure)
            {
                return NotFound(existingCustomer.Errors);
            }

            var result = await _customerService.UpdateCustomerAsync(customer);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer.IsFailure)
            {
                return NotFound(existingCustomer.Errors);
            }

            var result = await _customerService.DeleteCustomerAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }
    }
}