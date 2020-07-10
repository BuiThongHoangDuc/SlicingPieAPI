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

        public StackHoldersController(IStakeHolderService shService, SWDSlicingPieContext context, ISliceAssetService sliceService)
        {
            _shService = shService;
            _context = context;
            _sliceService = sliceService;
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
        [HttpGet("list")]
        public async Task<IActionResult> getContribute(String assetID)
        {

            //var Contribution = await _context.SliceAssets
            //                                .Where(asset => asset.AssetStatus == Status.ACTIVE && asset.AssetId == assetID)
            //                                .Select(asset => new SliceAssetDetailStringDto
            //                                {
            //                                    AssetId = asset.AssetId,
            //                                    AccountName = asset.Account.StakeHolders
            //                                                   .Where(sh => sh.CompanyId == asset.CompanyId && sh.AccountId == asset.AccountId)
            //                                                   .Select(sh => sh.ShnameForCompany).FirstOrDefault(),
            //                                    AssetSlice = asset.AssetSlice,
            //                                    CompanyName = asset.Company.CompanyName,
            //                                    Description = asset.Description,
            //                                    MultiplierInTime = asset.MultiplierInTime,
            //                                    ProjectName = asset.Project.ProjectName,
            //                                    Quantity = asset.Quantity,
            //                                    TermName = asset.Term.TermName,
            //                                    TimeAsset = asset.TimeAsset,
            //                                    TypeAssetName = asset.TypeAsset.NameAsset,
            //                                }).FirstOrDefaultAsync();
            return Ok();
        }



        //// GET: api/StackHolders/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<StackHolder>> GetStackHolder(string id)
        //{
        //    var stackHolder = await _context.StackHolders.FindAsync(id);

        //    if (stackHolder == null)
        //    {
        //        return NotFound();
        //    }

        //    return stackHolder;
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
