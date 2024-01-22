using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public class TransactionService
    {
        private readonly MoneyMinderDataContext _context;

        public TransactionService(MoneyMinderDataContext context)
        {
                _context = context;
        }

        public Transaction OneOffTransaction(Account account, int transactionCategory, string? description, decimal amount)
        {
            // Create transaction
            var transaction = new Transaction();

            transaction.FkAccountId = account.Id;
            transaction.FkTransactionCategoryId = transactionCategory;
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.Description = description;
            transaction.Amount = amount;
            transaction.Balance = account.Amount - amount;


            // Deduct amount from account total
            account.Amount -= amount;

            return transaction;
        } 
    }
}
