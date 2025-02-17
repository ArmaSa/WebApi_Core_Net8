using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.ViewModel.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.ViewModel
{
    public class PaymentInputViewModel
    {
        public long CustomerId { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethods PaymentMethod { get; set; }
        [Required]
        public long InvoiceId { get; set; }
    }
}
