using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class AutomaticPayment
{
    public int Id { get; set; }

    public int? FkAccountId { get; set; }

    public int? FkFrequencyId { get; set; }

    public int? FkCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Account? FkAccount { get; set; }

    public virtual TransactionCategory? FkCategory { get; set; }

    public virtual PaymentFrequency? FkFrequency { get; set; }
}
