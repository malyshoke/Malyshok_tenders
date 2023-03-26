using LR2_Malyshok.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LR2_Malyshok.Models.DTOClasses;

namespace LR2_Malyshok.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly TendersDataContext _context;

        public CompaniesController(TendersDataContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {
            if (_context.Company == null)
            {
                return NotFound();
            }
            return await _context.Company.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            if (_context.Company == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }
            CompanyDto companydto = (CompanyDto)company;
            return companydto;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTenders(int id)
        {
            var companyTenders = await _context.Tender.Where(t => t.OwnerId == id).ToListAsync();

            if (companyTenders == null)
            {
                return NotFound();
            }

            return companyTenders;
        }

        [HttpGet]
        [Route("{Name}")]
        public async Task<ActionResult<IEnumerable<String>>> GetCompanyStartsWith(String name)
        {
            //var companyNames = await _context.Company.Where(c => c.CompanyName.StartsWith(name)).Select(c => c.CompanyName).ToListAsync();
            var companyNames = await _context.Company.Where(c => c.CompanyName == name).Select(c => c.CompanyName).ToListAsync();

            if (companyNames == null)
            {
                return NotFound();
            }

            return companyNames;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<String>>> GetCompanyNames()
        {
            var companyNames = await _context.Company
                .Select(c => c.CompanyName)
                .ToListAsync();

            if (companyNames == null)
            {
                return NotFound();
            }

            return companyNames;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Tendering>>> GetCompanyTenderings(int id)
        {
            var companyTenderings = await _context.Tendering.Where(t => t.CompanyId == id).ToListAsync();

            if (companyTenderings == null)
            {
                return NotFound();
            }

            return companyTenderings;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(Company company)
        //{
        //  if (_context.Company == null)
        //  {
        //      return Problem("Entity set 'TendersDataContext.Company'  is null.");
        //  }
        //    _context.Company.Add(company);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        //}

        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(CompanyDto companydto)
        {
            if (_context.Company == null)
            {
                return Problem("Entity set 'TendersDataContext.Company'  is null.");
            }
            Company company = (Company)companydto;
            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }


        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (_context.Company == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return (_context.Company?.Any(e => e.CompanyId == id)).GetValueOrDefault();
        }

    }
}
