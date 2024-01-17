﻿using System;
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
    public class TransactionCategoriesController : ControllerBase
    {
        private readonly MoneyMinderDataContext _context;

        public TransactionCategoriesController(MoneyMinderDataContext context)
        {
            _context = context;
        }

        // GET: api/TransactionCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionCategory>>> GetTransactionCategories()
        {
          if (_context.TransactionCategories == null)
          {
              return NotFound();
          }
            return await _context.TransactionCategories.ToListAsync();
        }

        // GET: api/TransactionCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionCategory>> GetTransactionCategory(int id)
        {
          if (_context.TransactionCategories == null)
          {
              return NotFound();
          }
            var transactionCategory = await _context.TransactionCategories.FindAsync(id);

            if (transactionCategory == null)
            {
                return NotFound();
            }

            return transactionCategory;
        }

        // PUT: api/TransactionCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionCategory(int id, TransactionCategory transactionCategory)
        {
            if (id != transactionCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionCategoryExists(id))
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

        // POST: api/TransactionCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionCategory>> PostTransactionCategory(TransactionCategory transactionCategory)
        {
          if (_context.TransactionCategories == null)
          {
              return Problem("Entity set 'MoneyMinderDataContext.TransactionCategories'  is null.");
          }
            _context.TransactionCategories.Add(transactionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionCategory", new { id = transactionCategory.Id }, transactionCategory);
        }

        // DELETE: api/TransactionCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionCategory(int id)
        {
            if (_context.TransactionCategories == null)
            {
                return NotFound();
            }
            var transactionCategory = await _context.TransactionCategories.FindAsync(id);
            if (transactionCategory == null)
            {
                return NotFound();
            }

            _context.TransactionCategories.Remove(transactionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionCategoryExists(int id)
        {
            return (_context.TransactionCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
