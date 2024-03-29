﻿using LR2_Malyshok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static LR2_Malyshok.Models.DTOClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LR2_Malyshok.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TendersController : ControllerBase
    {
        private readonly TendersDataContext _context;

        public TendersController(TendersDataContext context)
        {
            _context = context;
        }

        // GET: api/Tenders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tender>>> GetTender()
        {
            if (_context.Tender == null)
            {
                return NotFound();
            }
            return await _context.Tender.ToListAsync();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<String>>> GetTendersSorted()
        {
            var tendersorted = await _context.Tender.OrderBy(t => t.TenderBudget).Select(t => t.TenderName).ToListAsync();
            if (tendersorted == null)
            {
                return NotFound();
            }
            return tendersorted;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Tender>>> GetTendersName(string Name)
        {
            var tendername = await _context.Tender.Where(t => t.TenderName.Contains(Name)).ToListAsync();
            if (tendername == null)
            {
                return NotFound();
            }
            return tendername;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetWinner(int id)
        {
            var tender = await _context.Tender.FindAsync(id);
            if (tender == null)
            {
                return NotFound();
            }
            var winningTendering = await _context.Tendering.Where(t => t.TenderId == id).OrderBy(t => t.CurrentBid).FirstOrDefaultAsync();
            if (winningTendering == null)
            {
                return NotFound();
            }
            var winner = await _context.Company.FindAsync(winningTendering.CompanyId);
            return (CompanyDto)winner;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tender>>> GetOpenTenders()
        {
            var openTenders = await _context.Tender.Where(t => t.TenderStart <= DateTime.UtcNow && t.TenderEnd >= DateTime.UtcNow).ToListAsync();
            //var openTenders = await _context.Tender.Where(t => t.IsOpen()).ToListAsync();
            if (openTenders == null)
            {
                return NotFound();
            }

            return openTenders;
        }

        // GET: api/Tenders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenderDto>> GetTender(int id)
        {
            if (_context.Tender == null)
            {
                return NotFound();
            }
            var tender = await _context.Tender.FindAsync(id);

            if (tender == null)
            {
                return NotFound();
            }
            return (TenderDto)tender;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTenderingByTender(int id)
        {
            if (_context.Tender == null)
            {
                return NotFound();
            }
            var tender = await _context.Tender
                .Include(t => t.Tenderings)
                .ThenInclude(t => t.Company)
                .FirstOrDefaultAsync(t => t.TenderId == id);

            if (tender == null)
            {
                return NotFound();
            }
            return Ok(tender?.Tenderings.Select(t=>new { 
                company_id = t.CompanyId,
                company = t.Company.CompanyName,
                bid = t.CurrentBid
            }));
        }

        // PUT: api/Tenders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutTender(int id, Tender tender)
        {
            if (id != tender.TenderId)
            {
                return BadRequest();
            }

            _context.Entry(tender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderExists(id))
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
        [HttpPut("{id},{days}")]
        public async Task<IActionResult> ExtendTender(int id, int days)
        {
            var tender = await _context.Tender.FindAsync(id);
            if (id != tender.TenderId)
            {
                return BadRequest();
            }
            tender.ExtendTenderPeriod(days);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderExists(id))
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudgetTender(int id, float amount)
        {
            var tender = await _context.Tender.FindAsync(id);
            if (tender == null)
            {
                return NotFound();
            }
            tender.UpdateBudget(amount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderExists(id))
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
        // POST: api/Tenders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tender>> PostTender(TenderDto tenderdto)
        {
            if (_context.Tender == null)
            {
                return Problem("Entity set 'TendersDataContext.Tender'  is null.");
            }
            var tender = (Tender)tenderdto;
            var company = await _context.Company.FindAsync(tender.CompanyId);
            if (company == null)
            {
                return NotFound();
            }
            company.AddTender(tender);
            _context.Tender.Add(tender);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTender", new { id = tender.TenderId }, tender);
        }

        // DELETE: api/Tenders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTender(int id)
        {
            if (_context.Tender == null)
            {
                return NotFound();
            }
            var tender = await _context.Tender.FindAsync(id);
            if (tender == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(tender.CompanyId);
            if (company == null)
            {
                return NotFound();
            }
            company.DeleteTender(tender);
            _context.Tender.Remove(tender);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenderExists(int id)
        {
            return (_context.Tender?.Any(e => e.TenderId == id)).GetValueOrDefault();
        }

    }
}
