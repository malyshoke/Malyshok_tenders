using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LR2_Malyshok.Models;
using NuGet.Versioning;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace LR2_Malyshok.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TenderingsController : ControllerBase
    {
        private readonly TendersDataContext _context;

        public TenderingsController(TendersDataContext context)
        {
            _context = context;
        }

        // GET: api/Tenderings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tendering>>> GetTendering()
        {
            if (_context.Tendering == null)
            {
                return NotFound();
            }
            return await _context.Tendering.ToListAsync();
        }

        // GET: api/Tenderings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenderingDto>> GetTendering(int id)
        {
            if (_context.Tendering == null)
            {
                return NotFound();
            }
            var tendering = await _context.Tendering.FindAsync(id);

            if (tendering == null)
            {
                return NotFound();
            }
            TenderingDto tenderingDto = (TenderingDto)tendering;
            return tenderingDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(int id, float bid)
        {
            var tendering = await _context.Tendering.FindAsync(id);
            if (tendering == null)
            {
                return NotFound();
            }
            tendering.PlaceBet(bid);
            //_context.Entry(tendering).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderingExists(id))
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

        // PUT: api/Tenderings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutTendering(int id, Tendering tendering)
        {
            if (id != tendering.TenderingId)
            {
                return BadRequest();
            }

            _context.Entry(tendering).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderingExists(id))
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

        // POST: api/Tenderings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tendering>> PostTendering(TenderingDto tenderingdto)
        {
            if (_context.Tendering == null)
            {
                return Problem("Entity set 'TendersDataContext.Tendering'  is null.");
            }
            Tendering tendering = (Tendering)tenderingdto;
            var company = await _context.Company.FindAsync(tendering.CompanyId);
            if (company == null)
            {
                return NotFound();
            }
            tendering.Company = company;
            company.PartInTendering(tendering);
            var tender = await _context.Tender.FindAsync(tendering.TenderId);
            if (tender == null)
            {
                return NotFound();
            }
            tendering.Tender = tender;
            tender.AddTendering(tendering);
            _context.Tendering.Add(tendering);
            await _context.SaveChangesAsync();
            if (tendering.CurrentBid > tender.TenderBudget)
            {
                return Problem("Entity set 'TendersDataContext.Tendering'  is null.");
            }

            return CreatedAtAction("GetTendering", new { id = tendering.TenderingId }, tendering);
        }

        // DELETE: api/Tenderings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTendering(int id)
        {
            if (_context.Tendering == null)
            {
                return NotFound();
            }
            var tendering = await _context.Tendering.FindAsync(id);
            if (tendering == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(tendering.CompanyId);
            company.CancelTendering(tendering);
            var tender = await _context.Tender.FindAsync(tendering.TenderId);
            tender.RemoveTendering(tendering);
            _context.Tendering.Remove(tendering);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenderingExists(int id)
        {
            return (_context.Tendering?.Any(e => e.TenderingId == id)).GetValueOrDefault();
        }
    }
}
