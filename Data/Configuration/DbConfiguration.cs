using InvoiceAppWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceWebApi.Data.Configuration
{
    public static class DbConfiguration
    {
        public static void AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<InvoiceDbContext>(option =>
            {
                option.UseInMemoryDatabase("TestDb");
            });
        }
    }
}
