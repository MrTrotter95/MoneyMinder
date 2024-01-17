using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class PaymentFrequency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AutomaticPayment> AutomaticPayments { get; set; } = new List<AutomaticPayment>();
}
