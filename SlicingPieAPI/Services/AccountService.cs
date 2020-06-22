using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _account;

        public AccountService(IAccountRepository account) {
            _account = account;
        }

        public List<object> getAccount(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected)
        {
            var accounts = _account.Search(name);
            accounts = _account.Sort(accounts, sort_Type);
            accounts = _account.Paging(accounts, page_Index, itemPerPage);
            List<Object> list = _account.Filter(accounts, field_Selected);
            return list;
        }

        public async Task<UserLoginDto> getAccountInfo(string email) {
            return await _account
                            .GetLoginInfo(email);
        }


    }

    public interface IAccountService
    {
        Task<UserLoginDto> getAccountInfo(string email);

        List<Object> getAccount(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected);
    }
}
