using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.DataAccessLayer.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts();
        Task<Account> GetAccount(int accountId);

    }
}
