using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SWD_SlicingPieContext _context;
        private readonly IAccountService _accountService;
        private const int ITEM_PER_PAGE = 5;
        public AccountsController(SWD_SlicingPieContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(string name = "",
            int page_index = -1,
            string sort_type = "",
            string field_selected = "")
        {
            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "AccountId, NameAccount, EmailAccount, PhoneAccount, StatusId, RoleId";

            var list = _accountService.getAccount(name, page_index, ITEM_PER_PAGE, sort_type, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);
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

        //    // PUT: api/Accounts/5
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutAccount(string id, Account account)
        //    {
        //        if (id != account.AccountId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(account).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AccountExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/Accounts
        //    [HttpPost]
        //    public async Task<ActionResult<Account>> PostAccount(Account account)
        //    {
        //        _context.Accounts.Add(account);
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            if (AccountExists(account.AccountId))
        //            {
        //                return Conflict();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        //    }

        //    // DELETE: api/Accounts/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<Account>> DeleteAccount(string id)
        //    {
        //        var account = await _context.Accounts.FindAsync(id);
        //        if (account == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Accounts.Remove(account);
        //        await _context.SaveChangesAsync();

        //        return account;
        //    }

        //    private bool AccountExists(string id)
        //    {
        //        return _context.Accounts.Any(e => e.AccountId == id);
        //    }
        }
    }

