using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LR2_Malyshok.Models;

namespace LR2_Malyshok.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<Tendering>> GetTendering(int id)
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

            return tendering;
        }

        // PUT: api/Tenderings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        public async Task<ActionResult<Tendering>> PostTendering(Tendering tendering)
        {
          if (_context.Tendering == null)
          {
              return Problem("Entity set 'TendersDataContext.Tendering'  is null.");
          }
            _context.Tendering.Add(tendering);
            await _context.SaveChangesAsync();

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
