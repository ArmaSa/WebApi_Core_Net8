using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.ViewModel.BaseModel;
using InvoiceAppWebApi.ViewModel.Customer;
using InvoiceAppWebApi.ViewModel.Invoice;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.ViewModel
{
    public class PaymentViewModel : BaseViewModel
    {
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethods PaymentMethod { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }

        public long InvoiceId { get; set; }
        [Required]
        public long customerId { get; set; }
        public InvoiceViewModel Invoice { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
