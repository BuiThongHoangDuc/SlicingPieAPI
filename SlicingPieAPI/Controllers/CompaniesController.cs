using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _company;
        private const int ITEM_PER_PAGE = 5;
        public CompaniesController(ICompanyService company)
        {
            _company = company;
        }

        // GET: api/Companies
        [HttpGet]
        public ActionResult GetCompanies(
            string name = "",
            int page_index = -1,
            string sort_type = "",
            string field_selected = "")
        {
            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "CompanyID, CompanyName, Comapnyicon, NonCashMultiplier, CashMultiplier";

            var list = _company.getListCompany(name, page_index, ITEM_PER_PAGE, sort_type, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);
        }

        // GET: api/Companies/5/List Stake holder
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}/stake-holder")]
        public async Task<ActionResult<IEnumerable<SHLoadMainDto>>> GetListSHCompany(string id)
        {
            
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var shID = claim[0].Value;
            var role = Convert.ToInt32(claim[1].Value);

            if (role == Role.MANAGER)
            {
                var info = await _company.getListSHComapny(id);
                if (info.ToList().Count == 0) return NotFound();
                return Ok(info.ToList());
            }
            else if (role == Role.EMPLOYEE)
            {
                var UserInfo = _company.getSHByCompany(id, shID).Result;
                return Ok(UserInfo);
            }
            return NotFound();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDetailDto>> GetCompany(string id)
        {
            var companyDetail = await _company.getDetailCompany(id);
            return companyDetail;
        }


        //// GET: api/Accounts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Account>> GetAccount(string id)
        //{
        //    var account = await _context.Accounts.FindAsync(id);

        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return account;
        //}

        //// PUT: api/Companies/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCompany(string id, Company company)
        //{
        //    if (id != company.CompanyId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(company).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompanyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Companies
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(Company company)
        //{
        //    _context.Companies.Add(company);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CompanyExists(company.CompanyId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        //}

        //// DELETE: api/Companies/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Company>> DeleteCompany(string id)
        //{
        //    var company = await _context.Companies.FindAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Companies.Remove(company);
        //    await _context.SaveChangesAsync();

        //    return company;
        //}

        //private bool CompanyExists(string id)
        //{
        //    return _context.Companies.Any(e => e.CompanyId == id);
        //}
    }
}
