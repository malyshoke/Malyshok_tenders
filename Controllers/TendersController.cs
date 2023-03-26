using LR2_Malyshok.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<String>>> GetTendersSorted()
        {
            var tendersorted = await _context.Tender.OrderBy(t => t.TenderBudget).Select(t => t.TenderName).ToListAsync();
            if (tendersorted == null)
            {
                return NotFound();
            }
            return tendersorted;
        }

        [HttpGet("{id}")]
        public IActionResult GetWinner(int id)
        {
            var tender = _context.Tender.FirstOrDefault(t => t.TenderId == id);
            if (tender == null)
            {
                return NotFound();
            }
            var winnerId = tender.SelectWinner();
            if (winnerId == 0)
            {
                return NotFound();
            }
            var winner = tender.Tenderings.FirstOrDefault(t => t.CompanyId == winnerId);
            if (winner == null)
            {
                return NotFound();
            }
            return Ok(winner);
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
        public async Task<ActionResult<Tender>> GetTender(int id)
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

            return tender;
        }

        // PUT: api/Tenders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Tenders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tender>> PostTender(TenderDto tenderdto)
        {
            if (_context.Tender == null)
            {
                return Problem("Entity set 'TendersDataContext.Tender'  is null.");
            }
            Tender tender = new Tender();
            tender = (Tender)tenderdto;
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
