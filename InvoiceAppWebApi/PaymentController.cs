using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAppWebApi.App
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        public PaymentController(IPaymentService paymentService, IMapper mapper) 
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("GetAllPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                List<PaymentViewModel> customers = await _paymentService.GetItemsAsync();

                if (customers == null)
                {
                    return NotFound(new { Message = "Payment not found" });
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("GetPaymentByInvoiceId")]
        public async Task<IActionResult> GetPaymentByInvoiceId(long invoiceId , long customerId)
        {
            try
            {
                List<string> includes = new();
                includes.Add(nameof(Customer));
                includes.Add(nameof(Invoice));

                PaymentViewModel result = await _paymentService.GetPaymentByInvoiceIdAsync(invoiceId, customerId, includes: includes.ToArray());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("AddPayment")]
        public async Task<ServiceResultModel<PaymentViewModel>> AddPaymentAsync(PaymentInputViewModel model)
        {
            ServiceResultModel<PaymentViewModel> resultInfo = new();
            try
            {
                if(!ModelState.IsValid)
                {
                    resultInfo.Messages = ConstantValue.PaymentModelNotValid;
                    return resultInfo;
                }

                resultInfo = await _paymentService.AddPaymentInvoiceCustomer(model);
                return resultInfo;
            }
            catch (Exception ex)
            {
                resultInfo.Messages = ConstantValue.PaymentFailed;
                return resultInfo;
            }
        }
    }
}
