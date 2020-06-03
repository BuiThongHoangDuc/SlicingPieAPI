using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.Models;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackHoldersController : ControllerBase
    {
        private readonly SWD_SlicingPieProjectContext _context;

        public StackHoldersController(SWD_SlicingPieProjectContext context)
        {
            _context = context;
        }

        // GET: api/StackHolders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StackHolder>>> GetStackHolders()
        {
            return await _context.StackHolders.ToListAsync();
        }

        // GET: api/StackHolders/5
        [HttpGet("GetStackHolderDetail/{id}")]
        public async Task<ActionResult<StackHolder>> GetStackHolderDetail(string username,  string password)
        {
            //var stackHolder = _context.StackHolders
            //                                    .Include(stholder => stholder.StackHolerDetails)
            //                                        .ThenInclude(stholderdt => stholderdt.Company)
            //                                    .Include(stholder => stholder.Assets)
            //                                        .ThenInclude(asset => asset.TypeAsset)
            //                                    .Where(stholder => stholder.StackHolerId == id)
            //                                    .FirstOrDefault();
            var stackHolder = _context.StackHolders.Where(stholder => stholder.StackHolerId == username).Select(stholder => new StackHolder
            {
                StackHolerId = stholder.StackHolerId,
                Shname = stholder.Shname,
                StatusId =stholder.StatusId,
            });
            if (stackHolder == null)
            {
                return NotFound();
            }

            return Ok(stackHolder);
        }

        // GET: api/StackHolders/5
        [HttpGet("PostStackHolderDetail/")]
        public async Task<ActionResult<StackHolder>> PostStackHolderDetail()
        {
            var StackHolders = new StackHolder();
            StackHolders.StackHolerId = "0";
            StackHolders.Shaccount = "duc";
            StackHolders.Shpassword = "1";
            StackHolders.StatusId = "2";
            StackHolders.RoleId = 2;

            StackHolerDetail stdetail = new StackHolerDetail();
            stdetail.StackHolerId = StackHolders.StackHolerId;
            stdetail.CompanyId = "BS101";
            stdetail.ShmarketSalary = 8000000;
            stdetail.Shsalary = 4000000;

            StackHolerDetail stdetail1 = new StackHolerDetail();
            stdetail1.StackHolerId = StackHolders.StackHolerId;
            stdetail1.CompanyId = "BS102";
            stdetail1.ShmarketSalary = 8000000;
            stdetail1.Shsalary = 5000000;

            StackHolders.StackHolerDetails.Add(stdetail);
            StackHolders.StackHolerDetails.Add(stdetail1);

            _context.StackHolders.Add(StackHolders);
            _context.SaveChanges();

            var stackHolder = _context.StackHolders
                                                .Include(stholder => stholder.StackHolerDetails)
                                                    .ThenInclude(stholderdt => stholderdt.Company)
                                                .Include(stholder => stholder.Assets)
                                                    .ThenInclude(asset => asset.TypeAsset)
                                                .Where(stholder => stholder.StackHolerId == StackHolders.StackHolerId)
                                                .FirstOrDefault();
            if (stackHolder == null)
            {
                return NotFound();
            }

            return stackHolder;
        }

        // GET: api/StackHolders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StackHolder>> GetStackHolder(string id)
        {
            var stackHolder = await _context.StackHolders.FindAsync(id);

            if (stackHolder == null)
            {
                return NotFound();
            }

            return stackHolder;
        }

        //Login
        // GET: api/StackHolders/5
        [HttpGet("GetAccount")]
        public async Task<ActionResult<StackHolder>> GetStackHolder(string username, string password)
        {
            //var stackHolder = await _context.StackHolders.Where(stholder => stholder.Shaccount == username && stholder.Shpassword == password)
            //                            .Select(stholder => new StackHolder {
            //                                Shname = stholder.Shname,
            //                                ShphoneNo = stholder.ShphoneNo,
            //                                StatusId = stholder.StatusId,
            //                                RoleId = stholder.RoleId,
            //                                Shemail = stholder.Shemail,
            //                            })
            //                            .FirstOrDefaultAsync();
            //if (stackHolder == null)
            //{
            //    return NotFound();
            //}

            //return stackHolder;

            var stackHolder = from c in _context.StackHolders
                              where (c.Shaccount == username && c.Shpassword == password)
                              select new { Shname = c.Shname, ShphoneNo = c.ShphoneNo, StatusId = c.StatusId, RoleId = c.RoleId, Shemail = c.Shemail};
            
            await stackHolder.FirstOrDefaultAsync();

            if (stackHolder == null)
            {
                return NotFound();
            }

            return Ok(stackHolder);

        }

        // PUT: api/StackHolders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStackHolder(string id, StackHolder stackHolder)
        {
            if (id != stackHolder.StackHolerId)
            {
                return BadRequest();
            }

            _context.Entry(stackHolder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StackHolderExists(id))
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

        // POST: api/StackHolders
        [HttpPost]
        public async Task<ActionResult<StackHolder>> PostStackHolder(StackHolder stackHolder)
        {
            _context.StackHolders.Add(stackHolder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StackHolderExists(stackHolder.StackHolerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStackHolder", new { id = stackHolder.StackHolerId }, stackHolder);
        }

        // DELETE: api/StackHolders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StackHolder>> DeleteStackHolder(string id)
        {
            var stackHolder = await _context.StackHolders.FindAsync(id);
            if (stackHolder == null)
            {
                return NotFound();
            }

            _context.StackHolders.Remove(stackHolder);
            await _context.SaveChangesAsync();

            return stackHolder;
        }

        private bool StackHolderExists(string id)
        {
            return _context.StackHolders.Any(e => e.StackHolerId == id);
        }
    }
}
