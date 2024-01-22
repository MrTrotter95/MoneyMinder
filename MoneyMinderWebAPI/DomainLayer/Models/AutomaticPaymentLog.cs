namespace MoneyMinderWebAPI.DomainLayer.Models
{
    public partial class AutomaticPaymentLog
    {
        public int Id { get; set; }

        public int? FKAutomaticPaymentId { get; set; }

        public DateTime TransactionDate { get; set; }

        public virtual AutomaticPayment? FKAutomaticPayment { get; set; }

    }
}
