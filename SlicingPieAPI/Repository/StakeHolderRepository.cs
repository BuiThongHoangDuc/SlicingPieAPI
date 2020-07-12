using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class StakeHolderRepository : IStakeHolderRepository
    {
        
        private readonly SWDSlicingPieContext _context;

        public StakeHolderRepository(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public async Task<string> getStakeHolderCompany(string id)
        {
            var companyID = await _context.StakeHolders
                                            .Where(sh => sh.AccountId == id && sh.Company.Status == Status.ACTIVE)
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
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            var ListMainUserInfo = await _context.StakeHolders
                                                    .Where(stInfo => stInfo.CompanyId == companyId)
                                                    .OrderBy(sh => Int32.Parse(re.Match(sh.AccountId).Groups[2].Value))
                                                    .Select(stInfo => new SHLoadMainDto
                                                    {
                                                        SHID = stInfo.AccountId,
                                                        SHName = stInfo.ShnameForCompany,
                                                        SHImage = stInfo.Shimage,
                                                        SHJob = stInfo.Shjob,
                                                        CompanyID = stInfo.CompanyId,
                                                        SliceAssets = stInfo.Account.SliceAssets
                                                                                                .Where(asset => asset.CompanyId == companyId && asset.AssetStatus == Status.ACTIVE)
                                                                                                .Select(asset => asset.AssetSlice).Sum() ?? 0
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

        public IQueryable<StakeHolerDetailDto> getStakeHolerDetail(string SHId)
        {
            return null;
        }

        public async Task<SalaryGapDto> GetSalaryGap(string userID, String companyID)
        {
            var salary = await _context.StakeHolders
                                    .Where(us => us.AccountId == userID && us.CompanyId == companyID && us.Shstatus == Status.ACTIVE)
                                    .Select(us => new SalaryGapDto{
                                        ShmarketSalary = us.ShmarketSalary,
                                        Shsalary = us.Shsalary,
                                    })
                                    .FirstOrDefaultAsync();
            return salary;
        }

        public async Task<string> GetNameStakeHolder(string userID, string companyID)
        {
            var Name = await _context.StakeHolders.Where(sh => sh.AccountId == userID && sh.CompanyId == companyID).Select(sh => sh.ShnameForCompany).FirstOrDefaultAsync();
            return Name;
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
        IQueryable<StakeHolerDetailDto> getStakeHolerDetail(String SHId);

        Task<SalaryGapDto> GetSalaryGap(String userID,String companyID);
        Task<String> GetNameStakeHolder(String userID, String companyID);
    }
}