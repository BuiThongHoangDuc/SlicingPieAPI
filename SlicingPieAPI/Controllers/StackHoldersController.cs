using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StackHoldersController : ControllerBase
    {

        private const int ITEM_PER_PAGE = 5;
        private readonly SWDSlicingPieContext _context;
        private readonly IStakeHolderService _shService;
        private readonly ISliceAssetService _sliceService;
        private readonly SheetsAPI _sheet;
        private readonly IStakeHolderService _shSV;

        public StackHoldersController(IStakeHolderService shService, SWDSlicingPieContext context, ISliceAssetService sliceService, SheetsAPI sheet, IStakeHolderService shSV)
        {
            _shService = shService;
            _context = context;
            _sliceService = sliceService;
            _sheet = sheet;
            _shSV = shSV;
        }

        [HttpGet]
        public IActionResult getStakeHolder(
            string name = "",
            string sort_type = "",
            int page_index = -1,
            string field_selected = "")
        {

            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "AccountId, ShnameForCompany, ShmarketSalary, Shsalary, Shjob, Shimage, Companyid";

            var list = _shService.getStakeHolder(name, sort_type, page_index, ITEM_PER_PAGE, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);
        }

        [HttpGet("{comapnyid}/{accountid}")]
        public async Task<ActionResult<AddStakeHolderDto>> GetListEquipmentInSC(String comapnyid, String accountid)
        {
            var sh = await _shService.GetShSV(comapnyid, accountid).FirstOrDefaultAsync();

            if (sh == null)
            {
                return NotFound();
            }

            return Ok(sh);
        }
        [HttpGet("list-company/{companyid}/{accountid}")]
        public async Task<ActionResult<AddStakeHolderDto>> GetListCompany(string companyid,String accountid)
        {
            var sh = await _shService.GetListCompanyStakeholder(companyid, accountid).ToListAsync();

            if (sh.Count == 0)
            {
                return NotFound();
            }

            return Ok(sh);
        }

        //PUT: api/Scenarios/5
        [HttpPut("{comapnyid}/{accountid}")]
        public async Task<ActionResult> PutScenarioStatus(String comapnyid, String accountid, AddStakeHolderDto editModel)
        {
            if (comapnyid != editModel.CompanyId && accountid != editModel.AccountId) return BadRequest();


            try
            {
                bool check = await _shService.UpdateShByIDSV(editModel);
                if (check == false) return NotFound();
                else return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //PUT: api/Scenarios/5
        [HttpDelete("{comapnyid}/{accountid}")]
        public async Task<ActionResult<int>> DeleteSh(String comapnyid, String accountid)
        {
            try
            {
                var check = await _shService.DeleteShByID(comapnyid, accountid);
                if (check == false) return NotFound();
                else
                {
                    //bool check2 = true;
                    bool check2 = await _sheet.DeleteEntry(comapnyid, comapnyid, accountid);
                    if (check2 == true)
                    {
                        await _sheet.UpdateEntry(comapnyid, comapnyid);
                        return NoContent();
                    }
                    else return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost("company/{id}")]
        public async Task<ActionResult<IEnumerable<SHLoadMainDto>>> AddSHCompany(string id, AddStakeHolderDto addModel)
        {
            if (id != addModel.CompanyId)
            {
                return BadRequest();
            }

            try
            {
                var check = await _shSV.AddStakeHolderSV(addModel);
                if (check == true)
                {
                    _sheet.CreateEntry(id, addModel.AccountId, addModel.ShnameForCompany, 0);
                    return NoContent();
                }
                else return BadRequest();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }


        ////GET: api/StackHolders/5
        //[HttpGet("{CompanyID}")]
        //public async Task<ActionResult> GetStackHolder(string CompanyID)
        //{
        //    var companyOver = await _context.Companies.Where(cp => cp.CompanyId == CompanyID && cp.Status == Status.ACTIVE)
        //                                    .Select(cp => new OverViewCompany
        //                                    {
        //                                        CompanyName = cp.CompanyName,
        //                                        CashPerSlice = cp.CashPerSlice,
        //                                        TotalSlice = cp.SliceAssets.Where(asset => asset.CompanyId == CompanyID && asset.AssetStatus == Status.ACTIVE).Select(asset => asset.AssetSlice).Sum() ?? 0,
        //                                        TotalStakeholder = cp.StakeHolders.Where(sh => sh.CompanyId == CompanyID && sh.Shstatus == Status.ACTIVE).Count(),
        //                                        TotalTerm = cp.TermSlice.Where(term => term.CompanyId == CompanyID).Count(),
        //                                    }).FirstOrDefaultAsync();
        //    return Ok(companyOver);
        //}


        //// PUT: api/StackHolders/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutStackHolder(string id, StackHolder stackHolder)
        //{
        //    if (id != stackHolder.StackHolerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(stackHolder).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StackHolderExists(id))
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

        //// POST: api/StackHolders
        //[HttpPost]
        //public async Task<ActionResult<StackHolder>> PostStackHolder(StackHolder stackHolder)
        //{
        //    _context.StackHolders.Add(stackHolder);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (StackHolderExists(stackHolder.StackHolerId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetStackHolder", new { id = stackHolder.StackHolerId }, stackHolder);
        //}

        //// DELETE: api/StackHolders/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<StackHolder>> DeleteStackHolder(string id)
        //{
        //    var stackHolder = await _context.StackHolders.FindAsync(id);
        //    if (stackHolder == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.StackHolders.Remove(stackHolder);
        //    await _context.SaveChangesAsync();

        //    return stackHolder;
        //}

        //private bool StackHolderExists(string id)
        //{
        //    return _context.StackHolders.Any(e => e.StackHolerId == id);
        //}
    }
}
