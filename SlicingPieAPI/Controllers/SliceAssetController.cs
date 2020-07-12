using Microsoft.AspNetCore.Mvc;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliceAssetController : ControllerBase
    {
        private readonly SWDSlicingPieContext _context;
        private readonly IStakeHolderService _shService;
        private readonly ISliceAssetService _sliceService;
        private readonly SheetsAPI _sheet;

        public SliceAssetController(IStakeHolderService shService, SWDSlicingPieContext context, ISliceAssetService sliceService, SheetsAPI sheet)
        {
            _shService = shService;
            _context = context;
            _sliceService = sliceService;
            _sheet = sheet;
        }

        // GET: api/Slice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SliceAssetDetailStringDto>> GetAssetById(string id)
        {
            var Contribution = await _sliceService.GetSliceByIDSV(id);
            if (Contribution == null) return NotFound();
            else return Ok(Contribution);
        }


        // PUT: api/Contribution/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(string id, SliceAssetDetailStringDto asset)
        {
            if (id != asset.AssetId)
            {
                return BadRequest();
            }

            try
            {
                bool check = await _sliceService.UpdateAssetSV(id, asset);
                if (check)
                { await _sheet.UpdateEntry("BS101"); return NoContent(); }
                else return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            };
        }

        //DELETE: api/Asset/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(String id)
        {
            try
            {
                var isDelete = await _sliceService.DeleteAssetSV(id);
                if (isDelete) return NoContent();
                else return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

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
