using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMinderWebAPI.DataAccessLayer.Context;
using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentFrequenciesController : ControllerBase
    {
        private readonly MoneyMinderDataContext _context;

        public PaymentFrequenciesController(MoneyMinderDataContext context)
        {
            _context = context;
        }

        // GET: api/PaymentFrequencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentFrequency>>> GetPaymentFrequencies()
        {
          if (_context.PaymentFrequencies == null)
          {
              return NotFound();
          }
            return await _context.PaymentFrequencies.ToListAsync();
        }

        // GET: api/PaymentFrequencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentFrequency>> GetPaymentFrequency(int id)
        {
          if (_context.PaymentFrequencies == null)
          {
              return NotFound();
          }
            var paymentFrequency = await _context.PaymentFrequencies.FindAsync(id);

            if (paymentFrequency == null)
            {
                return NotFound();
            }

            return paymentFrequency;
        }

        // PUT: api/PaymentFrequencies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentFrequency(int id, PaymentFrequency paymentFrequency)
        {
            if (id != paymentFrequency.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentFrequency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentFrequencyExists(id))
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

        // POST: api/PaymentFrequencies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentFrequency>> PostPaymentFrequency(PaymentFrequency paymentFrequency)
        {
          if (_context.PaymentFrequencies == null)
          {
              return Problem("Entity set 'MoneyMinderDataContext.PaymentFrequencies'  is null.");
          }
            _context.PaymentFrequencies.Add(paymentFrequency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentFrequency", new { id = paymentFrequency.Id }, paymentFrequency);
        }

        // DELETE: api/PaymentFrequencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentFrequency(int id)
        {
            if (_context.PaymentFrequencies == null)
            {
                return NotFound();
            }
            var paymentFrequency = await _context.PaymentFrequencies.FindAsync(id);
            if (paymentFrequency == null)
            {
                return NotFound();
            }

            _context.PaymentFrequencies.Remove(paymentFrequency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentFrequencyExists(int id)
        {
            return (_context.PaymentFrequencies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
