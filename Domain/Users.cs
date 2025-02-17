using Microsoft.AspNetCore.Identity;

namespace InvoiceAppWebApi.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }

}
