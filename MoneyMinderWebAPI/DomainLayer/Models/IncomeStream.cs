namespace MoneyMinderWebAPI.DomainLayer.Models
{
    public partial class IncomeStream
    {
        public int Id { get; set; }
        public int? FKFrequencyId { get; set; }
        public string? PersonsName { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public virtual PaymentFrequency? FkFrequency { get; set; }

    }
}
