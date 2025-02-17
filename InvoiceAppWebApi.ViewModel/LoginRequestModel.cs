using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAppWebApi.ViewModel
{
    public class LoginRequestModel
    {
        [Required]
        [DefaultValue("admin@example.com")]
        public string Email { get; set; }
        [Required]
        [DefaultValue("Admin@123")]
        public string Password { get; set; }
    }
}
