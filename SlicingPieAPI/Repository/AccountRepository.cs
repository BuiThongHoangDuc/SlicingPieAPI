using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly SWDSlicingPieContext _context;
        public AccountRepository(SWDSlicingPieContext context) {
            _context = context;
        }

        public async Task<bool> CreateAccount(AddAccountDto addModel)
        {
            int count = GetCountAccount().Result;
            String accountID = "Account" + count;
            Account accountModel = new Account();
            accountModel.AccountId = accountID;
            accountModel.NameAccount = addModel.NameAccount;
            accountModel.EmailAccount = addModel.EmailAccount;
            accountModel.PhoneAccount = addModel.PhoneAccount;
            accountModel.StatusId = Status.ACTIVE;
            accountModel.RoleId = Role.USER;
            
            

            _context.Accounts.Add(accountModel);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e.InnerException.Message);
                throw;
            }
        }

        private async Task<int> GetCountAccount()
        {
            var count = await _context.Accounts.CountAsync();
            return count;
        }

        public List<object> Filter(IQueryable<Account> account, string selectedField)
        {
            var list_query = account.ToList();

            List<Object> list_return = new List<Object>();
            SupportSelectField supportSelectField = new SupportSelectField();
            foreach (var item in list_query)
            {
                var temp = supportSelectField.getByField(item, selectedField);
                list_return.Add(temp);
            }
            return list_return;
        }

        public async Task<UserLoginDto> GetLoginInfo(string email)
        {
            var loginInfo = await _context.Accounts
                                                .Where(ac => ac.EmailAccount == email && ac.StatusId == Status.ACTIVE)
                                                .Select(ac => new UserLoginDto
                                                {
                                                    AccountID = ac.AccountId,
                                                    NameAccount = ac.NameAccount,
                                                    RoleID = ac.RoleId
                                                }).FirstOrDefaultAsync();
            return loginInfo;
        }

        public IQueryable<Account> Paging(IQueryable<Account> account, int page_index, int ITEM_PER_PAGE)
        {
            if (page_index != -1)
            {
                account = account.Skip(page_index * ITEM_PER_PAGE).Take(ITEM_PER_PAGE);

            }
            return account;
        }

        public IQueryable<Account> Search(string search)
        {
            IQueryable<Account> account = _context.Accounts.Where(q => q.NameAccount.Contains(search));
            return account;
        }

        public IQueryable<Account> Sort(IQueryable<Account> account, string typeOfSort)
        {
            switch (typeOfSort)
            {
                case "asc": account = account.OrderBy(p => p.AccountId); break;
                case "des": account = account.OrderByDescending(p => p.AccountId); break;
            }
            return account;
        }

        public async Task<bool> DeleteAccount(String id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return false;
            }
            try
            {
                account.StatusId = Status.INACTIVE;
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public IQueryable<AccountDetailDto> GetDetailAccount(String id)
        {
            var account = _context.Accounts
                                    .Where(ac => ac.AccountId == id && ac.StatusId == Status.ACTIVE)
                                    .Select(ac => new AccountDetailDto
                                    {
                                        AccountId = ac.AccountId,
                                        NameAccount = ac.NameAccount,
                                        EmailAccount = ac.EmailAccount,
                                        PhoneAccount = ac.PhoneAccount,
                                    });
            return account;
        }

        public async Task<String> UpdateScenario(String id, AccountDetailDto account)
        {
            Account accountModel = await _context.Accounts.FindAsync(id);
            if (accountModel == null) return null;
            accountModel.NameAccount = account.NameAccount;
            accountModel.PhoneAccount = account.PhoneAccount;

            _context.Entry(accountModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return accountModel.AccountId;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }

    public interface IAccountRepository
    {
        Task<UserLoginDto> GetLoginInfo(string email);

        IQueryable<Account> Search(string search);

        IQueryable<Account> Paging(IQueryable<Account> account, int pageIndex, int itemPerPage);

        IQueryable<Account> Sort(IQueryable<Account> account, string typeOfSort);

        List<Object> Filter(IQueryable<Account> account, string selectedField);
        Task<bool> CreateAccount(AddAccountDto addModel);
        Task<bool> DeleteAccount(String id);
        IQueryable<AccountDetailDto> GetDetailAccount(String id);
        Task<String> UpdateScenario(String id, AccountDetailDto account);


    }
}
