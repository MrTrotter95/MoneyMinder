using Microsoft.EntityFrameworkCore;
using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DomainLayer.Models;
using static MoneyMinderWebAPI.Helpers.Constants;

namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public class AutomaticPaymentService : IAutomaticPaymentService
    {
        private readonly MoneyMinderDataContext _context;

        public AutomaticPaymentService(MoneyMinderDataContext context)
        {
            _context = context;
        }

        async Task<List<AutomaticPayment>> GetAccountsAutomaticPayments(int accountID)
        {
            var accounts = await _context.AutomaticPayments.Where(ap => ap.FkAccountId == accountID).ToListAsync();

            if (accounts.Count == 0)
            {
                throw new Exception();
            }

            return accounts;
        }

        async Task<Account> GetAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null)
            {
                throw new Exception();
            }

            return account;
        }


        /// <summary>
        /// Generates a list of expenses that are due/late based on the provided account ID.
        /// </summary>
        /// <param name="accountId">The identifier of the financial account.</param>
        /// <returns>A list of expenses that are due today or earlier.</returns>
        public List<ExpenseToPay> GetDueAutomaticPayments(int accountID)
        {
            var automaticPayments = _context.AutomaticPayments
                .Where(ap => ap.FkAccountId == accountID)
                .ToList();

            var automaticPaymentIDs = automaticPayments.Select(ap => ap.Id).ToList();

            var recentPaymentLogs = _context.AutomaticPaymentLog
                .Where(apl => automaticPaymentIDs.Contains(apl.FKAutomaticPaymentId.Value))
                .OrderByDescending(apl => apl.TransactionDate) // Order by TransactionDate in descending order
                .GroupBy(apl => apl.FKAutomaticPaymentId)
                .Select(group => group.First()) // Only take the most recent log for each group
                .ToList();

            var endOfToday = DateTime.UtcNow.AddDays(1).AddTicks(-1);
            var expensesToPay = new List<ExpenseToPay>();

            foreach (var payment in automaticPayments)
            {
                var mostRecentLog = recentPaymentLogs.FirstOrDefault(log => log.FKAutomaticPaymentId == payment.Id);

                // Calculate upcoming expenses based on its most recent transactionDate or if it's a first time payment, then take it from the initial AP.StartDate
                var startDate = mostRecentLog?.TransactionDate ?? payment.LastPaymentDate;

                while (startDate <= endOfToday)
                {
                    var generatedExpense = GenerateExpenseToPay(payment, startDate.Value);

                    
                    if (generatedExpense.DueDate <= endOfToday)
                    {
                        expensesToPay.Add(generatedExpense);
                    }

                    startDate = generatedExpense.DueDate;
                }
            }


            return expensesToPay;
        }

        List<ExpenseToPay> IAutomaticPaymentService.GenerateExpensesUntilNextPay(int accountID, DateTime nextPayDate)
        {
            var expenses = new List<ExpenseToPay>();

            var endOfToday = DateTime.UtcNow.AddDays(1).AddTicks(-1);

            var automaticPayments = _context.AutomaticPayments
                .Where(ap => ap.FkAccountId == accountID)
                .ToList();

            foreach (var payment in automaticPayments)
            {
                var dueDate = payment.LastPaymentDate;

                while (dueDate < nextPayDate)
                {
                    var generatedExpense = GenerateExpenseToPay(payment, dueDate.Value);

                    if (generatedExpense.DueDate < nextPayDate)
                    {
                        expenses.Add(generatedExpense);
                    }

                    dueDate = generatedExpense.DueDate;
                }
            }

            return expenses;
        }

        /// <summary>
        /// Returns an ExpenseToPay with a new DueDate based on it's FrequencyID
        /// </summary>
        /// <param name="automaticPayment">The automaticPayment we are copying.</param>
        /// <param name="priorDueDate">This will either be the APs dueDate because it's a first time payment. Or, the most recent transactionlog transaction date.</param>
        public ExpenseToPay GenerateExpenseToPay(AutomaticPayment automaticPayment, DateTime priorDueDate)
        {
            var generatedPayment = new ExpenseToPay();

            generatedPayment.AccountID = automaticPayment.FkAccountId;
            generatedPayment.CategoryID = automaticPayment.FkCategoryId;
            generatedPayment.AutomaticPaymentID = automaticPayment.Id;
            generatedPayment.Name = automaticPayment.Name;
            generatedPayment.Amount = automaticPayment.Amount;
            generatedPayment.isPaid = false;

            switch (automaticPayment.FkFrequencyId)
            {
                case PaymentFrequencies.Daily:
                    generatedPayment.DueDate = priorDueDate.AddDays(1);
                    break;
                case PaymentFrequencies.Weekly:
                    generatedPayment.DueDate = priorDueDate.AddDays(7);
                    break;
                case PaymentFrequencies.Fortnightly:
                    generatedPayment.DueDate = priorDueDate.AddDays(14);
                    break;
                case PaymentFrequencies.Monthly:
                    generatedPayment.DueDate = priorDueDate.AddMonths(1);
                    break;
                default:
                    break;
            }

            return generatedPayment;
        }

        public Transaction GenerateTransactionFromExpense(ExpenseToPay expense)
        {
            return new Transaction()
            {
                FkAccountId = expense.AccountID,
                FkTransactionCategoryId = expense.CategoryID,
                TransactionDate = expense.DueDate,
                Description = expense.Name,
                Amount = expense.Amount,
            };
        }

        public Transaction DeductTransactionFromAccount(Transaction transaction)
        {
            var account = _context.Accounts.Find(transaction.FkAccountId);

            if (account == null)
            {
                throw new Exception();
            }

            account.Amount -= transaction.Amount;

            return transaction;
        }

        public AutomaticPaymentLog GenerateAPLogFromExpense(ExpenseToPay expense)
        {
            var apLog = new AutomaticPaymentLog()
            {
                FKAutomaticPaymentId = expense.AutomaticPaymentID,
                TransactionDate = expense.DueDate
            };

            return apLog;
        }
    }
    public class ExpenseToPay
    {
        public int? AccountID;
        public int? CategoryID;
        public int? AutomaticPaymentID;
        public string? Name;
        public decimal Amount;
        public DateTime DueDate;
        public bool isPaid;
    }
}
