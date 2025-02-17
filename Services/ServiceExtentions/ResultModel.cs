using InvoiceAppWebApi.Common;

namespace InvoiceAppWebApi.Services.ServiceExtentions
{
    public class ResultDuplicateModel : ServiceResultInfo
    {
        public ResultDuplicateModel()
        {
            Messages = "Duplicate Item found!";
            Succeeded = false;
        }
    }

    public class ResultPaymentModel : ServiceResultInfo
    {
        public ResultPaymentModel()
        {
            Messages = "Payment is not valid!";
            Succeeded = false;
        }
    }

    public class ResultPaymentcustomerNotFuondModel : ServiceResultInfo
    {
        public ResultPaymentcustomerNotFuondModel()
        {
            Messages = "Payment customer not found!";
            Succeeded = false;
        }
    }
}
