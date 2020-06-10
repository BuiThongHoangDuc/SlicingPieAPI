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
    public class StackHoldersController : ControllerBase
    {

        private const int ITEM_PER_PAGE = 5;
        private readonly SWD_SlicingPieContext _context;
        private readonly IStakeHolderService _shService;
        public StackHoldersController(IStakeHolderService shService, SWD_SlicingPieContext context)
        {
            _shService = shService;
            _context = context;

        }

        //GET: api/StackHolders
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("list-company-stake-holder")]
        public IActionResult GetStackHolders()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();

            var shID = claim[0].Value;
            var role = Convert.ToInt32(claim[1].Value);
            var CompanyID = claim[2].Value;

            if (role == Role.MANAGER)
            {
                var UserInfo = _shService.getListSHByCompany(CompanyID).Result;
                return Ok(UserInfo);
            }
            else if (role == Role.EMPLOYEE)
            {
                var UserInfo = _shService.getSHByCompany(CompanyID, shID).Result;
                return Ok(UserInfo);
            }
            return NotFound();
        }


        [HttpGet]
        public IActionResult getStakeHolder(
            string name = "",
            int page_index = -1,
            string sort_type = "",
            string field_selected = "")
        {

            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "AccountId, ShnameForCompany, ShmarketSalary, Shsalary, Shjob, Shimage, Companyid";

            var list = _shService.getStakeHolder(name, page_index, ITEM_PER_PAGE, sort_type, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);

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
