using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AccountsController : ControllerBase
    {
        private readonly SWDSlicingPieContext _context;
        private readonly IAccountService _accountService;
        private const int ITEM_PER_PAGE = 5;
        public AccountsController(SWDSlicingPieContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        public ActionResult<Account> GetAccounts(string name = "",
            string sort_type = "",
            int page_index = -1,
            string field_selected = "")
        {
            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "AccountId, NameAccount, EmailAccount, PhoneAccount, StatusId, RoleId";

            var list = _accountService.getAccount(name, sort_type, page_index, ITEM_PER_PAGE, field_selected);
            if (list.Count == 0) { return NotFound(); }
            else return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> PostAccount(AddAccountDto addModel)
        {
            try
            {
                var check = await _accountService.CreateAccountSV(addModel);
                if (check == true) return NoContent();
                else return BadRequest();
            }
            catch (Exception)
            {
                return Conflict();
            }
            
        }

        //DELETE: api/Scenarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteScenario(String id)
        {
            try
            {
                var isDelete = await _accountService.DeleteAccountSV(id);
                if (isDelete) return NoContent();
                else return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetailDto>> GetAccount(String id)
        {
            var account = await _accountService.GetDetailAccountSV(id).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        //PUT: api/Scenarios/5
        [HttpPut("{id}")]
        public async Task<ActionResult<String>> PutScenario(String id, AccountDetailDto account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            try
            {
                String idUpdate = await _accountService.UpdateScenarioSV(id, account);
                if (idUpdate == null) return NotFound();
                else return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



        //// GET: api/Accounts/5
        //[HttpGet("list")]
        //public async Task<ActionResult> GetAccountlist(string id)
        //{
        //    var account = await _context.StakeHolders.Where(sh => sh.CompanyId == id).Select((r,i) => new {count = i, name = r.AccountId }).ToListAsync();

        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(account);
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

        // POST: api/Accounts 


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

