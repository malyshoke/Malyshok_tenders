﻿using LR2_Malyshok.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static LR2_Malyshok.Models.DTOClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTendersTenderings(int id)
        {
            var company = await _context.Company.Include(t => t.CompanyTenders).ThenInclude(t => t.Tenderings).FirstOrDefaultAsync(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTendersInclude(int id)
        {
            var company = await _context.Company.Include(t => t.CompanyTenders).FirstOrDefaultAsync(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            var companyTenders = company.CompanyTenders;
            return Ok(companyTenders);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTendersByBudget(int id, float budget)
        {
            var company = await _context.Company.Include(t => t.CompanyTenders).FirstOrDefaultAsync(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            var companyTenders = company.CompanyTenders.Where(t => t.TenderBudget < budget);
            return Ok(companyTenders);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTendersByName(string name)
        {
            var company = await _context.Company.Include(t => t.CompanyTenders).Where(c => c.CompanyName.Contains(name)).FirstOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var companyTenders = company.CompanyTenders;
            return Ok(companyTenders);
        }

        //[HttpGet]
        //[Route("{id}")]
        //public async Task<ActionResult<IEnumerable<Tender>>> GetCompanyTenders(int id)
        //{
        //    var companyTenders = await _context.Tender.Where(t => t.CompanyId == id).ToListAsync();
        //    if (companyTenders == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return companyTenders;
        //    }
        //}


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
        [Authorize(Roles = "admin")]
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
