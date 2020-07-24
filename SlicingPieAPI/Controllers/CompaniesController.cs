﻿using System;
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
        private readonly ISliceAssetService _slice;
        private readonly IStakeHolderService _shSV;
        private readonly SheetsAPI _sheet;

        private const int ITEM_PER_PAGE = 5;

        public CompaniesController(ICompanyService company, ISliceAssetService slice, SheetsAPI sheet, IStakeHolderService shSV)
        {
            _company = company;
            _slice = slice;
            _sheet = sheet;
            _shSV = shSV;
        }

        // GET: api/Companies
        [HttpGet]
        public ActionResult GetCompanies(
            string name = "",
            string sort_type = "",
            int page_index = -1,
            string field_selected = "")
        {
            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "CompanyID, CompanyName, Comapnyicon, NonCashMultiplier, CashMultiplier,CashPerSlice";

            var list = _company.getListCompany(name, sort_type, page_index, ITEM_PER_PAGE, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);
        }

        // GET: api/Companies/5/List Stake holder
        [HttpGet("{id}/stake-holder")]
        public async Task<ActionResult<IEnumerable<SHLoadMainDto>>> GetListSHCompany(string id)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var shID = claim[0].Value;
            var role = Convert.ToInt32(claim[1].Value);

            if (role == Role.MANAGER || role == Role.ADMIN || role == Role.EMPLOYEE)
            {
                var info = await _company.getListSHComapny(id);
                if (info.ToList().Count == 0) return NotFound();
                else return Ok(info.ToList());
            }
            //else if (role == Role.EMPLOYEE)
            //{
            //    var UserInfo = _company.getSHByCompany(id, shID).Result;
            //    if (UserInfo == null) return NotFound();
            //    else return Ok(UserInfo);
            //}
            return NotFound();
        }
        // GET: api/Companies/5/List Stake holder
        [HttpGet("{id}/stake-holder-inactive")]
        public async Task<ActionResult<IEnumerable<SHLoadMainDto>>> GetListSHCompanyInactive(string id)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var shID = claim[0].Value;
            var role = Convert.ToInt32(claim[1].Value);

            if (role == Role.MANAGER || role == Role.ADMIN || role == Role.EMPLOYEE)
            {
                var info = await _shSV.getListShByCompanyInactive(id);
                if (info.ToList().Count == 0) return NotFound();
                else return Ok(info.ToList());
            }
            //else if (role == Role.EMPLOYEE)
            //{
            //    var UserInfo = _company.getSHByCompany(id, shID).Result;
            //    if (UserInfo == null) return NotFound();
            //    else return Ok(UserInfo);
            //}
            return NotFound();
        }

        [HttpGet("{companyid}/stake-holder/{userid}")]
        public async Task<ActionResult<IEnumerable<SHLoadMainDto>>> GetListSHCompany(string companyid, String userid)
        {
            var Name = await _company.GetNameStakeHolderSV(userid, companyid);
            if (Name == null) return NotFound();
            else return Ok(Name);
        }
        [HttpGet("{companyid}/chart-company")]
        public async Task<ActionResult<string>> GetChartCompany(string companyid)
        {
            var chart = await _company.GetChartCompanySV(companyid).FirstOrDefaultAsync();
            if (chart == null) return NotFound();
            else return Ok(chart);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDetailDto>> GetCompany(string id)
        {
            var companyDetail = await _company.getDetailCompany(id);
            return companyDetail;
        }

        // GET: api/Companies/5
        [HttpGet("{id}/over-view")]
        public async Task<ActionResult<OverViewCompany>> GetCompanyOver(string id)
        {
            var companyOver = await _company.GetOverViewCompanySV(id).FirstOrDefaultAsync();
            if (companyOver == null) return BadRequest();
            else return Ok(companyOver);
        }


        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> EditCompany(string id, CompanyDetailDto company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }
            else
            {
                var companyId = await _company.updateCompany(id, company);
                return Ok(new { CompanyId = companyId });
            }
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<CompanyDetailDto>> PostCompany(CompanyDetailDto company)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var role = Convert.ToInt32(claim[1].Value);
            if (role.Equals(Role.EMPLOYEE)) return Forbid();
            else
            {
                try
                {
                    var companyinfo = await _company.CreateCompany(company);
                    return CreatedAtAction("GetCompany", new { id = companyinfo.CompanyId }, company);
                }
                catch (DbUpdateException)
                {
                    return Conflict();
                }
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCompany(string id)
        {
            var isDelete = _company.deleteCompany(id);
            if (isDelete == true) return NoContent();
            else return NotFound();
        }

        [HttpGet("{id}/project")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjectCompany(string id)
        {
            var listProject = await _company.getListProject(id).ToListAsync();
            if (listProject == null) return NotFound();
            else return Ok(listProject);
        }

        [HttpGet("term/{id}/project")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetTermProjectCompany(int id)
        {
            var listTerm = await _company.getTermProjectCompanySV(id);
            if (listTerm == null) return NotFound();
            else return Ok(listTerm);
        }

        [HttpPut("term-done/{id}")]
        public async Task<IActionResult> UpdateAsset(int id)
        {

            try
            {
                bool check = await _company.UpdateDoneTermSV(id);
                if (check)
                {return NoContent(); }
                else return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            };
        }

        [HttpGet("{id}/list-term")]
        public async Task<ActionResult<IEnumerable<TermDto>>> GetTermCompany(String id)
        {
            var listTerm = await _company.GetListTermCompanySV(id);
            if (listTerm == null) return NotFound();
            else return Ok(listTerm);
        }

        [HttpPost("{id}/project")]
        public async Task<ActionResult> CreateProject(String id, ProjectDto project)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var role = Convert.ToInt32(claim[1].Value);
            if (role.Equals(Role.EMPLOYEE)) return Forbid();
            else
            {
                try
                {
                    var check = await _company.AddProjectSV(id, project);
                    if (check == true)
                        return NoContent();
                    else return Conflict();
                }
                catch (DbUpdateException)
                {
                    return Conflict();
                }
            }
        }

        [HttpPut("{id}/project/{projectid}")]
        public async Task<ActionResult<CompanyDetailDto>> UpdateProject(String id, ProjectDto project, String projectid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var role = Convert.ToInt32(claim[1].Value);
            if (role.Equals(Role.EMPLOYEE)) return Forbid();
            else
            {
                if (id != project.CompanyId) return BadRequest();
                else if (projectid != project.ProjectId)
                {
                    return BadRequest();
                }
                else
                {
                    var check = await _company.udpateProjectSV(projectid, project);
                    if (check)
                        return NoContent();
                    else return Conflict();
                }
            }
        }

        [HttpDelete("{id}/project/{projectid}")]
        public IActionResult DeleteProject(String id, String projectid)
        {
            var isDelete = _company.deleteProjectSV(projectid);
            if (isDelete == true) return Ok(id);
            else return NotFound();
        }

        [HttpGet("{companyid}/term/{termid}/contribution")]
        public async Task<ActionResult> GetListContribution(String companyid, int termid)
        {
            var ListContribution = await _slice.GetListSlice(companyid, termid);
            if (ListContribution == null) return NotFound();
            else return Ok(ListContribution);
        }

        [HttpGet("{companyid}/term/{termid}/stake-holer/{shid}/contribution")]
        public async Task<ActionResult> GetListContributionSH(String companyid, String shid,int termid)
        {
            var ListContribution = await _slice.GetListSliceSHSV(companyid, shid, termid);
            if (ListContribution == null) return NotFound();
            else return Ok(ListContribution);
        }

        [HttpGet("{companyid}/type-asset")]
        public async Task<ActionResult> GetListTypeAsset(String companyid)
        {
            var ListAsset = await _company.GetListTypeAssetByCompanyIDSV(companyid);
            if (ListAsset == null) return NotFound();
            else return Ok(ListAsset);
        }

        [HttpPost("{companyid}/create-term")]
        public async Task<ActionResult> CreateTerm(String companyid, AddTermDto term)
        {
            if (companyid != term.CompanyId)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    bool check = await _company.addTermCompanySV(term);
                    if (check)
                    {
                        return NoContent();
                    }
                    else return BadRequest();
                }
                catch (DbUpdateException)
                {
                    return Conflict();
                }

            }
        }

        [HttpPost("term/{termid}/project/{projectid}")]
        public async Task<ActionResult> AddProjectToTerm(int termid, string projectid)
        {
            try
            {
                bool check = await _company.AddProjectToTermSV(termid, projectid);
                if (check)
                {
                    return NoContent();
                }
                else return BadRequest();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

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

        //    return NoContent();
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
