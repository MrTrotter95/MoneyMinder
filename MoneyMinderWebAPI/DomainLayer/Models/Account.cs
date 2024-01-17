using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class Account
{
    public int Id { get; set; }

    public int? FkAccountTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool HouseSavingsEnabled { get; set; }

    public virtual ICollection<AutomaticPayment> AutomaticPayments { get; set; } = new List<AutomaticPayment>();

    public virtual AccountType? FkAccountType { get; set; }

    public virtual ICollection<HouseSaving> HouseSavings { get; set; } = new List<HouseSaving>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
