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
    public class StakeHolderRepository : IStakeHolderRepository
    {
        private readonly string ACTIVE = "1";
        private readonly String INACTIVE = "2";
        private readonly String OUTCOMPANY = "3";

        private readonly SWD_SlicingPieContext _context;

        public StakeHolderRepository(SWD_SlicingPieContext context)
        {
            _context = context;
        }

        public async Task<string> getStakeHolderCompany(string id)
        {
            var companyID = await _context.StakeHolders
                                            .Where(sh => sh.AccountId == id)
                                            .Select(sh => sh.CompanyId)
                                            .FirstOrDefaultAsync();
            return companyID;
        }

        public async Task<StakeHolderDto> getSHLoginInfoByID(string id)
        {
            var shInfo = await _context.StakeHolders
                                    .Where(sh => sh.AccountId == id && sh.Shstatus == ACTIVE)
                                    .Select(sh => new StakeHolderDto { 
                                        SHID = sh.AccountId,
                                        CompanyID = sh.CompanyId,
                                        RoleID = sh.Shrole
                                    })
                                    .FirstOrDefaultAsync();
            return shInfo;
        }

        public async Task<IEnumerable<SHLoadMainDto>> getListShByCompany(string companyId)
        {

            var ListMainUserInfo = await _context.StakeHolders
                                                    .Where(stInfo => stInfo.CompanyId == companyId)
                                                    .Select(stInfo => new SHLoadMainDto
                                                    {
                                                        SHID = stInfo.AccountId,
                                                        SHName = stInfo.ShnameForCompany,
                                                        SHImage = stInfo.Shimage,
                                                        SHJob = stInfo.Shjob,
                                                        CompanyID = stInfo.CompanyId,
                                                        SliceAssets = stInfo.Account.SliceAssets
                                                                                                .Where(asset => asset.CompanyId == companyId)
                                                                                                .Select(asset => asset.AssetSlice).Sum()
                                                    })
                                                    .ToListAsync();
            return ListMainUserInfo;
        }

        public async Task<SHLoadMainDto> getShByIDCompany(string companyId, string userId)
        {
            var MainUserInfo = await _context.StakeHolders
                                                        .Where(stInfo => stInfo.CompanyId == companyId && stInfo.AccountId == userId)
                                                        .Select(stInfo => new SHLoadMainDto
                                                        {
                                                            SHID = stInfo.AccountId,
                                                            SHName = stInfo.ShnameForCompany,
                                                            SHImage = stInfo.Shimage,
                                                            SHJob = stInfo.Shjob,
                                                            CompanyID = stInfo.CompanyId,
                                                            SliceAssets = stInfo.Account.SliceAssets
                                                                                                    .Where(asset => asset.CompanyId == companyId)
                                                                                                    .Select(asset => asset.AssetSlice).Sum()
                                                        })
                                                        .FirstOrDefaultAsync();
            return MainUserInfo;
        }
    }

    public interface IStakeHolderRepository
    {
        Task<StakeHolderDto> getSHLoginInfoByID(string id);
        Task<string> getStakeHolderCompany(string id);
        Task<IEnumerable<SHLoadMainDto>> getListShByCompany(string companyId);
        Task<SHLoadMainDto> getShByIDCompany(string companyId, string userId);
    }
}