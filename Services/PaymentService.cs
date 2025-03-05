using AutoMapper;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.Services.Interface;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.Services.ServiceExtentions;
using InvoiceAppWebApi.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace InvoiceAppWebApi.Services
{
    public class PaymentService : CrudBase<Payment, PaymentViewModel>, IPaymentService, ITransient
    {
        private IUnitOfWork<Payment> _uow;
        private readonly DbSet<Payment> _payment;
        private readonly IMapper _mapper;
        private IQueryable<Payment> _query;
        public PaymentService(IUnitOfWork<Payment> unitOfWork, IMapper mapper, ILogger<ReadBase<Payment, PaymentViewModel>> logger, IOptions<ApplicationSettings> appSettings) : base(mapper, logger, unitOfWork,appSettings)
        {
            _uow = unitOfWork;
            _mapper = mapper;
            _payment = _uow.Set<Payment>();
            _query = _uow.AsQueryable<Payment>();
        }

        public override async Task<ServiceResultInfo> CreateAsync(PaymentViewModel modelItem, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            if(modelItem.Amount <=0 )
            {
                return new ResultPaymentModel();
            }
            var entityItem = _mapper.Map<Payment>(modelItem);
            await _entities.AddAsync(entityItem).ConfigureAwait(false);

            if (savechange)
            {
                await _uow.SaveChangesAsync(savechange).ConfigureAwait(false);

                if (entityItem.Id > 0)
                {
                    //modelItem.Id = entityItem.Id;
                    serviceResult.Id = entityItem.Id;
                    serviceResult.Succeeded = true;
                    serviceResult.Messages = "Payment created successfully!";
                }
                else
                {
                    serviceResult.Succeeded = false;
                    serviceResult.Messages = "Payment creation failed!";
                }
            }
            else
            {
                serviceResult.Succeeded = true;
            }

            return serviceResult;
        }

        public async Task<PaymentViewModel> GetPaymentByInvoiceIdAsync(long invoiceId , long customerId , params string[] includes)
        {
            Expression<Func<Payment, bool>> expression = p => p.Id == p.Id;
            _query = _query.AsNoTracking();

            var serviceResult = new ServiceResultInfo();
            if (invoiceId < 0)
            {
                new PaymentViewModel();
            }

            if (invoiceId > 0)            
                expression = expression.And(c => c.InvoiceId == invoiceId);            
            if(customerId > 0)
                expression = expression.And(c=>c.CustomerId == customerId || c.Invoice.CustomerId == customerId);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    _query = _query.Include(include);
                }
            }

            _query.Where(expression);

            var item = _query?.FirstOrDefault();

            return _mapper.Map<PaymentViewModel>(item);
        }

        public async Task<ServiceResultModel<PaymentViewModel>> AddPaymentInvoiceCustomer(PaymentInputViewModel paymentInput)
        {
            ServiceResultModel<PaymentViewModel> serviceResult = new();

            if(paymentInput.Amount <= 0)
            {
                serviceResult.Messages = ConstantValue.PaymentAmoundValid;
                return serviceResult;
            }

            try
            {
                Payment payemnt = _mapper.Map<Payment>(paymentInput);
                _payment.Add(payemnt);
                _uow.SaveChanges(true);
                serviceResult.Succeeded = true;
                serviceResult.Messages = ConstantValue.PaymentSuccessfuly;
                return serviceResult;
            }
            catch (Exception ex) 
            {
                serviceResult.Messages = ConstantValue.PaymentFailed;
                return serviceResult;
            }
        }
    }
}
