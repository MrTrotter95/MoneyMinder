using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class HouseSaving
{
    public int Id { get; set; }

    public int? FkAccountId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? GoalDate { get; set; }

    public decimal Amount { get; set; }

    public decimal? GoalAmount { get; set; }

    public virtual Account? FkAccount { get; set; }
}
