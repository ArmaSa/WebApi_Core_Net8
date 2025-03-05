using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.Services.ServiceExtentions;
using InvoiceAppWebApi.ViewModel;
using InvoiceAppWebApi.ViewModel.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Linq.Expressions;

namespace InvoiceAppWebApi.Services
{
    public class InvoiceService : CrudBase<Invoice, InvoiceViewModel>, IInvoiceService, ITransient
    {
        private IUnitOfWork<Invoice> _uow;
        private readonly DbSet<Invoice> _invoice;
        private readonly IMapper _mapper;
        private IQueryable<Invoice> _query;
        private ILogger<InvoiceService> _logger;

        public InvoiceService(IUnitOfWork<Invoice> unitOfWork, IMapper mapper, ILogger<InvoiceService> logger, IOptions<ApplicationSettings> appSettings) : base(mapper, logger, unitOfWork, appSettings)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _invoice = _uow.Set<Invoice>();
            _query = _uow.AsQueryable<Invoice>();
            _logger = logger;
        }

        public async Task<ServiceResultInfo> CreateAsync(InvoiceInputViewModel modelItem, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();
            var entityItem = _mapper.Map<Invoice>(modelItem);

            entityItem.InvoiceNumber = await GenarateInvoiceNumber();
            await _entities.AddAsync(entityItem).ConfigureAwait(false);

            if (savechange)
            {
                await _uow.SaveChangesAsync(savechange).ConfigureAwait(false);

                if (entityItem.Id > 0)
                {
                    //modelItem.Id = entityItem.Id;
                    serviceResult.Id = entityItem.Id;
                    serviceResult.Succeeded = true;
                    serviceResult.Messages = "Invoice created successfully!";
                }
                else
                {
                    serviceResult.Succeeded = false;
                    serviceResult.Messages = "Invoice creation failed!";
                }
            }
            else
            {
                serviceResult.Succeeded = true;
            }

            return serviceResult;
        }

        public async Task<ServiceResultModel<List<InvoiceViewModel>>> GetInvoiceByCustomerId(long customerId , params string[] includes)
        {
            ServiceResultModel<List<InvoiceViewModel>> serviceResult = new();
            Expression<Func<Invoice, bool>> expression = p => p.CustomerId == p.CustomerId;

            try
            {
                _query = _query.AsNoTracking();

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        _query = _query.Include(include);
                    }
                }

                _query.Where(expression);

                List<Invoice> invListData = await _query?.ToListAsync();
                var model = _mapper.Map<List<InvoiceViewModel>>(invListData);

                serviceResult.Result = model;
                serviceResult.Messages = ConstantValue.SuccessFulyMsg;
                return serviceResult;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                serviceResult.Messages = ConstantValue.FailedMsg;
                return serviceResult;
            }
        }

        private async Task<string> GenarateInvoiceNumber()
        {
            bool duplicatInvoiceNo = false;
            string invNumber = "";
            do
            {
                var randomNumber = new Random().Next(10000, 20000);
                invNumber = $"INVC{randomNumber}{DateTime.Now.Microsecond}";
                duplicatInvoiceNo = await _invoice.AnyAsync(c => c.InvoiceNumber == invNumber);

            } while (duplicatInvoiceNo);

            return invNumber;
        }

    }
}
