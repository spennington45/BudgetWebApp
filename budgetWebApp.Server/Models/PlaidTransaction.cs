using Going.Plaid.Entity;

namespace budgetWebApp.Server.Models
{
    public class PlaidTransaction
    {
        public string TransactionId { get; set; }
        public string PendingTransactionId { get; set; }
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string MerchantName { get; set; }
        public bool Pending { get; set; }
        public IList<string> Category { get; set; }
        public string OriginalDescription { get; set; }
        public string PaymentChannel { get; set; }
        public string TransactionCode { get; set; }
        public string Currency { get; set; }
        public string IsoCurrencyCode { get; set; }
        public string UnofficialCurrencyCode { get; set; }
        public Location Location { get; set; }
        public PaymentMeta PaymentMeta { get; set; }
    }
}
