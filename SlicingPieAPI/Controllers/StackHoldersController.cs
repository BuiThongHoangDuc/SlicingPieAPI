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
using SlicingPieAPI.Repository;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackHoldersController : ControllerBase
    {
        private readonly SWD_SlicingPieContext _context;
        private StackHolderRepository _UserRepository;
        public StackHoldersController(SWD_SlicingPieContext context)
        {
            _context = context;
            _UserRepository = new StackHolderRepository(context);
        }

        // GET: api/StackHolders
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetStackHolders()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var Role = Convert.ToInt32(claim[2].Value);
            var CompanyID = claim[3].Value;
            var UserID = claim[0].Value;
            //return Ok(new { role = Role, companyid = CompanyID, userid = UserID });
            if (Role == 2)
            {
                var UserInfo = _UserRepository.GetUserByCompany(CompanyID).Result;
                return Ok(UserInfo);
            }
            else if(Role == 3)
            {
                var UserInfo = _UserRepository.getUserByIDCompany(CompanyID, UserID);
                return Ok(UserInfo);
            }
            return NotFound();
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
