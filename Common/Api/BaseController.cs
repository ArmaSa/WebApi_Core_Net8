using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InvoiceAppWebApi.Common
{
    public class BaseController: ControllerBase
    {
        protected ClaimsIdentity claimsIdentity;

        protected readonly IOptions<ApplicationSettings> _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(IOptions<ApplicationSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        [NonAction]
        public string GetUserId()
        {
            claimsIdentity = (ClaimsIdentity)_httpContextAccessor?.HttpContext?.User?.Identity;
            return claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        [NonAction]
        public string GetUserName()
        {
            claimsIdentity = (ClaimsIdentity)_httpContextAccessor?.HttpContext?.User?.Identity;
            return claimsIdentity?.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name || c.Type == "preferred_username")?.Value;
        }
    }
}
