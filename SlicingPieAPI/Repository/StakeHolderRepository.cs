using Microsoft.AspNetCore.Mvc;
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
    public class StakeHolderRepository : IStakeHolderRepository
    {
        
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
                                    .Where(sh => sh.AccountId == id && sh.Shstatus == Status.ACTIVE)
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

        public IQueryable<StakeHolder> Search(string search)
        {
          IQueryable<StakeHolder> stakeHolders = _context.StakeHolders.Where(q => q.ShnameForCompany.Contains(search));
            return stakeHolders;
        }

        public IQueryable<StakeHolder> Paging(IQueryable<StakeHolder> stakeHolder, int page_index, int ITEM_PER_PAGE)
        {
            if (page_index != -1)
            {
                stakeHolder = stakeHolder.Skip(page_index * ITEM_PER_PAGE).Take(ITEM_PER_PAGE);

            }

            return stakeHolder;
        }

        public IQueryable<StakeHolder> Sort(IQueryable<StakeHolder> stakeHolder, string typeOfSort)
        {
            switch (typeOfSort)
            {
                case "asc": stakeHolder = stakeHolder.OrderBy(p => p.AccountId); break;
                case "des": stakeHolder = stakeHolder.OrderByDescending(p => p.AccountId); break;
            }
            return stakeHolder;
        }

        public List<Object> Filter(IQueryable<StakeHolder> stakeHolders, string selectedField)
        {
            var list_query = stakeHolders.ToList();

            List<Object> list_return = new List<Object>();
            SupportSelectField supportSelectField = new SupportSelectField();
            foreach (var item in list_query)
            {
                var temp = supportSelectField.getByField(item, selectedField);
                list_return.Add(temp);
            }
            return list_return;
        }
    }

    public interface IStakeHolderRepository
    {
        Task<StakeHolderDto> getSHLoginInfoByID(string id);
        Task<string> getStakeHolderCompany(string id);
        Task<IEnumerable<SHLoadMainDto>> getListShByCompany(string companyId);
        Task<SHLoadMainDto> getShByIDCompany(string companyId, string userId);

        IQueryable<StakeHolder> Search(string search);

        IQueryable<StakeHolder> Paging(IQueryable<StakeHolder> stakeHolder, int pageIndex, int itemPerPage);

        IQueryable<StakeHolder> Sort(IQueryable<StakeHolder> stakeHolder, string typeOfSort);

        List<Object> Filter(IQueryable<StakeHolder> stakeHolders, string selectedField);
            
    }
}