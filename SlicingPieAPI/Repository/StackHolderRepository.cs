using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class StackHolderRepository : SlicingPieAPI.Repository.IStackHolderRepository
    {
        private readonly SWD_SlicingPieContext _context;

        public StackHolderRepository(SWD_SlicingPieContext context)
        {
            _context = context;
        }

        public UserLoginDto GetUserInfo(string email)
        {
            
            var stackHoler = _context.StackHolders
                                                .Where(stHoler => stHoler.Shemail == email)
                                                .Select(stHolder => new UserLoginDto
                                                {
                                                    StackHolderID = stHolder.StackHolerId,
                                                    StatusID = stHolder.StatusId,
                                                    RoleID = stHolder.RoleId,
                                                    CompanyID = stHolder.StackHolerDetails.Where(stDt => stDt.Shdtstatus == "1").Select(stDt => stDt.CompanyId).FirstOrDefault()
                                                }).FirstOrDefault();
            return stackHoler;
        }

        public async Task<IEnumerable<MainDto>> GetUserByCompany(string companyId)
        {

                var ListMainUserInfo = await _context.StackHolerDetails
                                                        .Where(stInfo => stInfo.CompanyId == companyId)
                                                        .Select(stInfo => new MainDto
                                                        {
                                                            SHID = stInfo.StackHolerId,
                                                            SHName = stInfo.ShnameForCompany,
                                                            SHImage = stInfo.Shimage,
                                                            SHJob = stInfo.Shjob,
                                                            CompanyID = stInfo.CompanyId,
                                                            SliceAssets = stInfo.StackHoler.SliceAssets
                                                                                                    .Where(asset => asset.CompanyId == companyId)
                                                                                                    .Select(asset => asset.AssetSlice).Sum()
                                                        })
                                                        .ToListAsync();
                return ListMainUserInfo;
        }

        public MainDto getUserByIDCompany(string companyId, string userId)
        {
            var MainUserInfo = _context.StackHolerDetails
                                                        .Where(stInfo => stInfo.CompanyId == companyId && stInfo.StackHolerId == userId)
                                                        .Select(stInfo => new MainDto
                                                        {
                                                            SHID = stInfo.StackHolerId,
                                                            SHName = stInfo.ShnameForCompany,
                                                            SHImage = stInfo.Shimage,
                                                            SHJob = stInfo.Shjob,
                                                            CompanyID = stInfo.CompanyId,
                                                            SliceAssets = stInfo.StackHoler.SliceAssets
                                                                                                    .Where(asset => asset.CompanyId == companyId)
                                                                                                    .Select(asset => asset.AssetSlice).Sum()
                                                        })
                                                        .FirstOrDefault();
            return MainUserInfo;
        }
    }

    public interface IStackHolderRepository
    {
        Task<IEnumerable<MainDto>> GetUserByCompany(string companyId);
        UserLoginDto GetUserInfo(string email);
        MainDto getUserByIDCompany(string companyId, string userId);
    }
}