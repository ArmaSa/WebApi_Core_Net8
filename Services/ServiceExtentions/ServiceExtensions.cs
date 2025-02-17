using Autofac;
using InvoiceAppWebApi.Services;
using InvoiceAppWebApi.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Principal;

namespace InvoiceAppWebApi.FrameworkExtention
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            var servicesAssembly = typeof(CustomerService).Assembly;

            containerBuilder.RegisterAssemblyTypes(servicesAssembly)
                .AssignableTo<IScoped>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(servicesAssembly)
                .AssignableTo<ITransient>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(servicesAssembly)
                .AssignableTo<ISingleton>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
