using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class TransactionCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AutomaticPayment> AutomaticPayments { get; set; } = new List<AutomaticPayment>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
