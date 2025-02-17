using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAppWebApi.Common
{
    public class ApplicationSettings
    {
        public LogSetting Logging { get; set; }
        public JwtSetting Jwt  { get; set; }
        public  IdentitySet IdentitySettings { get; set; }
        public string AllowedHosts { get; set; }

        public class IdentitySet
        {
            public bool PasswordRequireDigit { get; set; }
            public int PasswordRequiredLength { get; set; }
            public bool PasswordRequireNonAlphanumeric { get; set; }
            public bool PasswordRequireUppercase { get; set; }
            public bool PasswordRequireLowercase { get; set; }
            public bool RequireUniqueEmail { get; set; }
        }

        public class JwtSetting
        {
            public string SecretKey { get; set; }
            public string EncryptKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string NotBeforeMinutes { get; set; }
            public string ExpirationMinutes { get; set; }
        }

        public class LogSetting
        {
            public LogLevel LogLevel { get; set; }
        }

        public class LogLevel
        {
            public string Default { get; set; }
            public string MicrosoftAspNetCore { get; set; }
        }
    }
}
