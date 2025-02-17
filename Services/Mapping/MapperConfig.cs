using AutoMapper;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.ViewModel;
using InvoiceAppWebApi.ViewModel.Customer;
using InvoiceAppWebApi.ViewModel.Invoice;
using InvoiceAppWebApi.ViewModel.InvoiceItem;

namespace InvoiceAppWebApi.Service.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(c => $"{c.FirstName}{c.LastName}"))
                .PreserveReferences().MaxDepth(2);

            CreateMap<CustomerViewModel, Customer>()
                  .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CustomerInputViewModel, CustomerViewModel>();

            CreateMap<PaymentInputViewModel, Payment>()
                 .ForMember(dest => dest.Invoice, opt => opt.Ignore());

            CreateMap<InvoiceInputViewModel, Invoice>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.SodorDate, opt => opt.MapFrom(src => src.SodorDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.PaidAmount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.InvoiceItem));

            CreateMap<InvoiceItemInputViewModel, InvoiceItem>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<Invoice, InvoiceViewModel>().PreserveReferences().MaxDepth(2);
            CreateMap<InvoiceItem, InvoiceItemViewModel>().PreserveReferences().MaxDepth(2);
            CreateMap<Payment, PaymentViewModel>().PreserveReferences().MaxDepth(2);
        }
    }
}
