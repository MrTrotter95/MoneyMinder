using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly MoneyMinderDataContext _context;
        private readonly IAutomaticPaymentService _automaticPaymentService;

        public AccountService(MoneyMinderDataContext context, IAutomaticPaymentService automaticPaymentService)
        {
            _context = context;
            _automaticPaymentService = automaticPaymentService;
        }

        async Task<List<Account>> IAccountService.GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();

            if (accounts.Count == 0)
            {
                throw new Exception("No Accounts found");
            }

            return accounts;
        }

        async Task<Account> IAccountService.GetAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            return account;
        }

        async Task<AccountBaseViewModel> IAccountService.GetAccountBaseViewModel(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            var expensesToPay = _automaticPaymentService.GenerateExpensesUntilNextPay(accountId, DateTime.UtcNow);

            var totalCost = expensesToPay.Sum(exp => exp.Amount);

            var viewModel = CreateViewModel(account, totalCost);

            return viewModel;
        }

        private AccountBaseViewModel CreateViewModel(Account account, decimal totalCost)
        {
            return new AccountBaseViewModel
            {
                AccountId = account.Id,
                AccountName = account.Name,
                CurrentTotal = account.Amount,
                ExpensesUntilNextPay = totalCost,
                Difference = account.Amount - totalCost
            };
        }

        public class AccountBaseViewModel
        {
            public int AccountId;
            public string? AccountName;
            public decimal CurrentTotal;
            public decimal ExpensesUntilNextPay;
            public decimal Difference;
        }


        List<Transaction> IAccountService.TransferFunds(Account senderAccount, Account recipientAccount, decimal amount)
        {
            var transferTransactionLogs = new List<Transaction>();

            // Deduct transfer amount from source account
            senderAccount.Amount -= amount;

            var senderAccountTransaction = new Transaction();

            senderAccountTransaction.FkAccountId = senderAccount.Id;
            senderAccountTransaction.Description = "Transfer";
            senderAccountTransaction.Amount = amount;
            senderAccountTransaction.Balance = senderAccount.Amount;


            // Add transfer amount to target account
            recipientAccount.Amount += amount;

            var recipientAccountTransaction = new Transaction();

            recipientAccountTransaction.FkAccountId = recipientAccount.Id;
            recipientAccountTransaction.Description = "Transfer";
            recipientAccountTransaction.Amount = amount;
            recipientAccountTransaction.Balance = recipientAccount.Amount;

            transferTransactionLogs.Add(senderAccountTransaction);
            transferTransactionLogs.Add(recipientAccountTransaction);

            return transferTransactionLogs;
        }
    }
}