namespace InvoiceAppWebApi.Common
{
    public class ServiceResultModel<TModel>: ServiceResultInfo where TModel : class
    {
        public ServiceResultModel()
        {
            Succeeded = false;
        }
        public TModel Result { get; set; }
    }

    public class ServiceResultInfo
    {

        public ServiceResultInfo()
        {
            Succeeded = false;
        }

        public Int64 Id { get; set; }

        public bool Succeeded { get; set; }

        public int TotalRecord { get; set; }

        public string Messages { get; set; }

    }
}