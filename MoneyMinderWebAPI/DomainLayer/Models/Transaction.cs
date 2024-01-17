using System;
using System.Collections.Generic;

namespace MoneyMinderWebAPI.DomainLayer.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int? FkAccountId { get; set; }

    public int? FkTransactionTypeId { get; set; }

    public int? FkTransactionCategoryId { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public virtual Account? FkAccount { get; set; }

    public virtual TransactionCategory? FkTransactionCategory { get; set; }

    public virtual TransactionType? FkTransactionType { get; set; }
}
