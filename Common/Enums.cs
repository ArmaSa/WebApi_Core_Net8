namespace InvoiceAppWebApi.Common
{
    public class Enums
    {
        public enum PaymentStatus
        {
            Pending = 1,
            Paid = 2,
        }

        public enum PaymentMethods
        {
            CardBeCard = 1,
            Cash = 2,
            HavaleBanki = 3,
        }
        public enum TrailType : byte
        {
            None = 0,
            Create = 1,
            Update = 2,
            Delete = 3
        }
    }
}
