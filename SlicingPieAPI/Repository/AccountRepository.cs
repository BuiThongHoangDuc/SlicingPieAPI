using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
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
    }

    public interface IAccountRepository
    {
        Task<UserLoginDto> GetLoginInfo(string email);

        IQueryable<Account> Search(string search);

        IQueryable<Account> Paging(IQueryable<Account> account, int pageIndex, int itemPerPage);

        IQueryable<Account> Sort(IQueryable<Account> account, string typeOfSort);

        List<Object> Filter(IQueryable<Account> account, string selectedField);
    }
}
