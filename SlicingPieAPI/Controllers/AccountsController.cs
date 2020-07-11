using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Helper;
using SlicingPieAPI.Models;
using SlicingPieAPI.Services;
using StackExchange.Redis;

namespace SlicingPieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SWDSlicingPieContext _context;
        private readonly IAccountService _accountService;
        private IDatabase _redisCache;
        private const int ITEM_PER_PAGE = 5;


        public AccountsController(SWDSlicingPieContext context, IAccountService accountService, IDatabase redisCache)
        {
            _context = context;
            _accountService = accountService;
            _redisCache = redisCache;
        }

        // GET: api/Accounts
        [HttpGet]
        public ActionResult<Account> GetAccounts(string name = "",
            string sort_type = "",
            int page_index = -1,
            string field_selected = "")
        {
            List<Object> list;
            if (string.IsNullOrEmpty(sort_type)) sort_type = "asc";
            if (string.IsNullOrEmpty(field_selected)) field_selected = "AccountId, NameAccount, EmailAccount, PhoneAccount, StatusId, RoleId";

            if(name == "" && sort_type == "asc" && page_index == -1 && field_selected == "AccountId, NameAccount, EmailAccount, PhoneAccount, StatusId, RoleId")
            {
                var result = RedisCacheHelper.Get("ListAccounts", _redisCache);

                if (result == null)
                {
                    list = _accountService.getAccount(name, sort_type, page_index, ITEM_PER_PAGE, field_selected);
                    
                    
                    if(list == null)
                    {
                        return NotFound();
                    } else
                    {
                        var data = JsonConvert.SerializeObject(list);
                        List<AccountDto> listDto = JsonConvert.DeserializeObject<List<AccountDto>>(data);

                        RedisCacheHelper.Set("ListAccounts", listDto, _redisCache);

                        return Ok(list);
                    }

                } else
                {        
                    return Ok(result);
                }
            } else
            {
                list = _accountService.getAccount(name, sort_type, page_index, ITEM_PER_PAGE, field_selected);
                if (list.Count == 0) { return NotFound(); }
                else return Ok(list);
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

