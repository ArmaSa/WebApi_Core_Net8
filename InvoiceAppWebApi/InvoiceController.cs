using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.ViewModel.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAppWebApi.App
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        public InvoiceController(IInvoiceService invoiceService, IMapper mapper) 
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet("GetCustomerById/{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            try
            {
                List<string> includes = new();
                includes.Add(nameof(Customer));
                includes.Add(nameof(Invoice.Items));
                includes.Add(nameof(Invoice.Payments));

                ServiceResultModel<List<InvoiceViewModel>> result = await _invoiceService.GetInvoiceByCustomerId(customerId , includes: includes.ToArray());

                if (result.Result?.Any() == false)
                {
                    return NotFound(new { Message = "Invoice not found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("AddInvoice")]
        public async Task<IActionResult> AddInvoice(InvoiceInputViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            ServiceResultInfo result =  await _invoiceService.CreateAsync(viewModel);

            return Ok(result);
        }

        [HttpDelete("DeleteInvoice{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0)
            {
                return BadRequest(new { Message = "Id not valid" });
            }

            ServiceResultInfo result = await _invoiceService.DeleteByKeysAsync(id);
            return Ok(result);
        }
    }
}
