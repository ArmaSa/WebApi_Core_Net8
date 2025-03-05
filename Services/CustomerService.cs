using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.Services.ServiceExtentions;
using InvoiceAppWebApi.ViewModel.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceAppWebApi.Services
{
    public class CustomerService : CrudBase<Customer, CustomerViewModel>, ICustomerService, ITransient
    {
        private IUnitOfWork<Customer> _uow;
        private readonly DbSet<Customer> _customer;
        private readonly IMapper _mapper;
        private readonly IOptions<ApplicationSettings> _appSettings;
        public CustomerService(IUnitOfWork<Customer> unitOfWork, IMapper mapper, ILogger<ReadBase<Customer, CustomerViewModel>> logger, IOptions<ApplicationSettings> appSettings) : base(mapper, logger, unitOfWork, appSettings)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _customer = _uow.Set<Customer>();
            _appSettings = appSettings;
        }

        public override async Task<ServiceResultInfo> CreateAsync(CustomerViewModel modelItem, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            bool hasDuplicate = await _customer.AnyAsync(c => modelItem.Email == c.Email);
            if(hasDuplicate)
            {
                return new ResultDuplicateModel();
            }
            var entityItem = _mapper.Map<Customer>(modelItem);
            await _entities.AddAsync(entityItem).ConfigureAwait(false);

            if (savechange)
            {
                await _uow.SaveChangesAsync(true).ConfigureAwait(false);

                if (entityItem.Id > 0)
                {
                    //modelItem.Id = entityItem.Id;
                    serviceResult.Id = entityItem.Id;
                    serviceResult.Succeeded = true;
                    serviceResult.Messages = "Customer created successfully!";
                }
                else
                {
                    serviceResult.Succeeded = false;
                    serviceResult.Messages = "Customer creation failed!";
                }
            }
            else
            {
                serviceResult.Succeeded = true;
            }

            return serviceResult;
        }

    }
}
