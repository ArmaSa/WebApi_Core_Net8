using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.ViewModel.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InvoiceAppWebApi.App
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private static IHttpContextAccessor _httpContextAccessor;

        public CustomerController(ICustomerService customerCrudService, IMapper mapper , IOptions<ApplicationSettings> appSetting, IHttpContextAccessor httpContextAccessor) :base(appSetting, httpContextAccessor) 
        {
            _customerService = customerCrudService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                List<CustomerViewModel> customers = await _customerService.GetItemsAsync();

                if (customers == null)
                {
                    return NotFound(new { Message = "Customers not found" });
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("GetCustomerById/{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            try
            {
                CustomerViewModel customer = await _customerService.GetItemByKeyAsync(customerId);

                if (customer == null)
                {
                    return NotFound(new { Message = "Customer not found" });
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerInputViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            CustomerViewModel model = _mapper.Map<CustomerViewModel>(viewModel);
            ServiceResultInfo result =  await _customerService.CreateAsync(model);

            return Ok(result);
        }

        [HttpDelete("Deletecustomer/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
            {
                return BadRequest(new { Message = "Id not valid" });
            }

            ServiceResultInfo result = await _customerService.DeleteByKeysAsync(id);
            return Ok(result);
        }

        [HttpGet("GetCustomerInvoice/{customerId}")]
        public async Task<IActionResult> GetCustomerInvoice(int customerId)
        {
            return Ok();
        }
    }
}
