using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string ACTIVE = "1";
        private readonly String INACTIVE = "2";
        private readonly SWD_SlicingPieContext _context;
        public AccountRepository(SWD_SlicingPieContext context) {
            _context = context;
        }

        public async Task<UserLoginDto> GetLoginInfo(string email)
        {
            var loginInfo = await _context.Accounts
                                                .Where(ac => ac.EmailAccount == email && ac.StatusId == ACTIVE)
                                                .Select(ac => new UserLoginDto
                                                {
                                                    AccountID = ac.AccountId,
                                                    RoleID = ac.RoleId
                                                }).FirstOrDefaultAsync();
            return loginInfo;
        }
    }

    public interface IAccountRepository
    {
        Task<UserLoginDto> GetLoginInfo(string email);
    }
}
