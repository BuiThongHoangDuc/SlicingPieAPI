using SlicingPieAPI.DTOs;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _acount;

        public AccountService(IAccountRepository account) {
            _acount = account;
        }

        public async Task<UserLoginDto> getAccountInfo(string email) {
            return await _acount
                            .GetLoginInfo(email);
        }


    }

    public interface IAccountService
    {
        Task<UserLoginDto> getAccountInfo(string email);
    }
}
