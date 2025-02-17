using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.ViewModel.BaseModel;
using InvoiceAppWebApi.ViewModel.Invoice;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAppWebApi.ViewModel.Customer
{
    public class CustomerViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "نام کاربری نمیتواند بیشتر از 100 کاراکتر باشد")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "فامیلی را وارد کنید")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "فامیلی کاربری نمیتواند بیشتر از 100 کاراکتر باشد")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "ایمیل را وارد کنید")]
        [EmailAddress(ErrorMessage = "ادرس ایمیل نا معتبر است")]
        public string Email { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<InvoiceViewModel> Invoices { get; set; } = new List<InvoiceViewModel>();
    }
}
