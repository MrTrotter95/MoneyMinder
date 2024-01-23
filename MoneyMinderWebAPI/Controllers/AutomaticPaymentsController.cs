using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DataAccessLayer.Services;
using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomaticPaymentsController : ControllerBase
    {
        private readonly MoneyMinderDataContext _context;
        private readonly IAutomaticPaymentService _automaticPaymentService;

        public AutomaticPaymentsController(MoneyMinderDataContext context, IAutomaticPaymentService automaticPaymentService)
        {
            this._context = context;
            this._automaticPaymentService = automaticPaymentService;
        }

        // GET: api/AutomaticPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutomaticPayment>>> GetAutomaticPayments()
        {
          if (_context.AutomaticPayments == null)
          {
              return NotFound();
          }
            return await _context.AutomaticPayments.ToListAsync();
        }

        // GET: api/AutomaticPayments/DuePayments/{accountID}
        [HttpGet("DuePayments/{accountID}")]
        public ActionResult<IEnumerable<ExpenseToPay>> GetDueAutomaticPayments(int accountID)
        {
            var expensesToPay = _automaticPaymentService.GetDueAutomaticPayments(accountID);

            return expensesToPay;
        }

        // POST: api/AutomaticPayments/PayAutomaticPayment/{accountID}
        [HttpPost("PayAutomaticPayment/{accountID}")]
        public IActionResult PayAutomaticPayment(ExpenseToPay expenseToPay)
        {
            var apService = new AutomaticPaymentService(_context);
            // CreateTransaction
            var partialTransaction = apService.GenerateTransactionFromExpense(expenseToPay);

            var completedTransaction = apService.DeductTransactionFromAccount(partialTransaction);

            _context.Transactions.Add(completedTransaction);
            _context.SaveChanges();

            // CreateAutomaticPaymentLog
            var automaticPaymentLog = apService.GenerateAPLogFromExpense(expenseToPay);
            _context.AutomaticPaymentLog.Add(automaticPaymentLog);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/AutomaticPayments/Generate/{accountID}
        [HttpGet("UntilNextPay/{accountID}")]
        public ActionResult<IEnumerable<ExpenseToPay>> GetAccountExpensesUntilNextPay(int accountID)
        {
            var apService = new AutomaticPaymentService(_context);

            var now = DateTime.UtcNow;

            var expensesToPay = _automaticPaymentService.GenerateExpensesUntilNextPay(accountID, now);

            return expensesToPay;
        }




        // GET: api/AutomaticPayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutomaticPayment>> GetAutomaticPayment(int id)
        {
          if (_context.AutomaticPayments == null)
          {
              return NotFound();
          }
            var automaticPayment = await _context.AutomaticPayments.FindAsync(id);

            if (automaticPayment == null)
            {
                return NotFound();
            }

            return automaticPayment;
        }

        // PUT: api/AutomaticPayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutomaticPayment(int id, AutomaticPayment automaticPayment)
        {
            if (id != automaticPayment.Id)
            {
                return BadRequest();
            }

            _context.Entry(automaticPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutomaticPaymentExists(id))
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

        [HttpPost]
        public async Task<ActionResult<AutomaticPayment>> PostAutomaticPayment(AutomaticPayment automaticPayment)
        {
          if (_context.AutomaticPayments == null)
          {
              return Problem("Entity set 'MoneyMinderDataContext.AutomaticPayments'  is null.");
          }
            _context.AutomaticPayments.Add(automaticPayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutomaticPayment", new { id = automaticPayment.Id }, automaticPayment);
        }

        // DELETE: api/AutomaticPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutomaticPayment(int id)
        {
            if (_context.AutomaticPayments == null)
            {
                return NotFound();
            }
            var automaticPayment = await _context.AutomaticPayments.FindAsync(id);
            if (automaticPayment == null)
            {
                return NotFound();
            }

            _context.AutomaticPayments.Remove(automaticPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutomaticPaymentExists(int id)
        {
            return (_context.AutomaticPayments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
