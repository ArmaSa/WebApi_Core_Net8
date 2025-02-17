using InvoiceAppWebApi.FrameworkExtention;

namespace InvoiceAppWebApi.ViewModel.BaseModel
{
    public class BaseViewModel : IKey64
    {
        public Int64 Id { get; set; }
    }
}
