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
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackHoldersController : ControllerBase
    {
        private readonly int MANAGER = 1;
        private readonly int EMPLOYEE = 2;
        private const int ITEM_PER_PAGE = 2;
        private readonly SWD_SlicingPieContext _context;
        private readonly IStakeHolderService _shService;
        public StackHoldersController(IStakeHolderService shService, SWD_SlicingPieContext context)
        {
            _shService = shService;
            _context = context;

        }

       // //GET: api/StackHolders
       //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
       //[HttpGet]
       // public IActionResult GetStackHolders()
       // {
       //     var identity = HttpContext.User.Identity as ClaimsIdentity;
       //     IList<Claim> claim = identity.Claims.ToList();

       //     var shID = claim[0].Value;
       //     var Role = Convert.ToInt32(claim[1].Value);
       //     var CompanyID = claim[2].Value;


       //     if (Role == MANAGER)
       //     {
       //         var UserInfo = _shService.getListSHByCompany(CompanyID).Result;
       //         return Ok(UserInfo);
       //     }
       //     else if (Role == EMPLOYEE)
       //     {
       //         var UserInfo = _shService.getSHByCompany(CompanyID, shID).Result;
       //         return Ok(UserInfo);
       //     }
       //     return NotFound();
       // }
        [HttpGet]
        public IActionResult FindByID(string id = "",
            string name="",
            string email="",
            int page_index=-1,
            string sort_type="aid",
            string field_selected="")
        {
            // "aid" asc id "did" des id
            // "aname" asc , "dname" des name
            var info = _context.Accounts.Where(p => p.NameAccount.Contains(name));
            if(!string.IsNullOrEmpty(id))
                info =  info.Where(p => p.AccountId.Equals(id));
            if (!string.IsNullOrEmpty(email))
                info =  info.Where(p => p.EmailAccount.Equals(email));

            //PAGING
            if (page_index != -1)
            {
                info = info.Skip(page_index * ITEM_PER_PAGE).Take(ITEM_PER_PAGE);
            }
            // SORT
            switch (sort_type)
            {
                case "aid": info = info.OrderBy(p => p.AccountId); break;
                case "did": info = info.OrderByDescending(p => p.AccountId); break;
            }

            var list_query = info.ToList();
            //if (list_query.Count <= 0) return NotFound();
            // SELECT FILED
            
            if (!string.IsNullOrEmpty(field_selected))
            {
                List<Object> list_return = new List<Object>();
                SupportSelectField supportSelectField = new SupportSelectField();
                foreach(var item in list_query)
                {
                    var temp = supportSelectField.getByField(item, field_selected);
                    list_return.Add(temp);
                }
                return Ok(list_return);
            }
            return Ok(list_query);
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
