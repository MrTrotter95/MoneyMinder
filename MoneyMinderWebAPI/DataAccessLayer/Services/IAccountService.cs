using MoneyMinderWebAPI.DomainLayer.Models;
using static MoneyMinderWebAPI.DataAccessLayer.Services.AccountService;

namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts();
        Task<Account> GetAccount(int accountId);
        Task<AccountBaseViewModel> GetAccountBaseViewModel(int accountId);
        List<Transaction> TransferFunds(Account senderAccount, Account recipientAccount, decimal amount);

    }
}
