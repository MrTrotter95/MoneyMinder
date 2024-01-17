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
    public class HouseSavingsController : ControllerBase
    {
        private readonly MoneyMinderDataContext _context;

        public HouseSavingsController(MoneyMinderDataContext context)
        {
            _context = context;
        }

        // GET: api/HouseSavings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HouseSaving>>> GetHouseSavings()
        {
          if (_context.HouseSavings == null)
          {
              return NotFound();
          }
            return await _context.HouseSavings.ToListAsync();
        }

        // GET: api/HouseSavings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HouseSaving>> GetHouseSaving(int id)
        {
          if (_context.HouseSavings == null)
          {
              return NotFound();
          }
            var houseSaving = await _context.HouseSavings.FindAsync(id);

            if (houseSaving == null)
            {
                return NotFound();
            }

            return houseSaving;
        }

        // PUT: api/HouseSavings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouseSaving(int id, HouseSaving houseSaving)
        {
            if (id != houseSaving.Id)
            {
                return BadRequest();
            }

            _context.Entry(houseSaving).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseSavingExists(id))
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

        // POST: api/HouseSavings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HouseSaving>> PostHouseSaving(HouseSaving houseSaving)
        {
          if (_context.HouseSavings == null)
          {
              return Problem("Entity set 'MoneyMinderDataContext.HouseSavings'  is null.");
          }
            _context.HouseSavings.Add(houseSaving);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHouseSaving", new { id = houseSaving.Id }, houseSaving);
        }

        // DELETE: api/HouseSavings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouseSaving(int id)
        {
            if (_context.HouseSavings == null)
            {
                return NotFound();
            }
            var houseSaving = await _context.HouseSavings.FindAsync(id);
            if (houseSaving == null)
            {
                return NotFound();
            }

            _context.HouseSavings.Remove(houseSaving);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HouseSavingExists(int id)
        {
            return (_context.HouseSavings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
