using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DataAccessLayer.Services;
using MoneyMinderWebAPI.DomainLayer.Models;
using static MoneyMinderWebAPI.DataAccessLayer.Services.AccountService;

namespace MoneyMinderWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly MoneyMinderDataContext _context;
        private readonly IAccountService accountService;

        public AccountsController(MoneyMinderDataContext context, IAccountService accountService)
        {
            this._context = context;
            this.accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }

            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // GET: api/Accounts/5
        [HttpGet("AccountSummary/{accountID}")]
        public async Task<ActionResult<AccountBaseViewModel>> GetAccountSummary(int accountID)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }

            var accountService = new AccountService(_context);

            var account = await accountService.GetAccountBaseViewMode(accountID);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
          if (_context.Accounts == null)
          {
              return Problem("Entity set 'MoneyMinderDataContext.Accounts'  is null.");
          }
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        // POST: api/Accounts/TransferFunds
        [HttpPost("TransferFunds")]
        public IActionResult TransferFunds(int senderID, int recipientID, decimal amount)
        {
            var senderAccount = _context.Accounts.Find(senderID);
            var recipientAccount = _context.Accounts.Find(recipientID);

            if (senderAccount == null || recipientAccount == null) { return NoContent(); }

            var transferService = new AccountService(_context);

            var transferTransactionLogs = transferService.TransferFunds(senderAccount, recipientAccount, amount);


            _context.Entry(senderAccount).State = EntityState.Modified;
            _context.Entry(recipientAccount).State = EntityState.Modified;
            

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(senderID) || !AccountExists(recipientID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            _context.Transactions.AddRange(transferTransactionLogs);
            _context.SaveChanges();


            return NoContent();
        }


    }
}
