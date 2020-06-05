using Microsoft.AspNetCore.Mvc;
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
                                                    CompanyID = stHolder.StackHolerDetails.Select(stDt => stDt.CompanyId).FirstOrDefault()
                                                }).FirstOrDefault();
            return stackHoler;
        }

        
        public IEnumerable<MainDto> GetUserByCompany(string companyId, int role)
        {
            if (role == 2)
            {
                var MainUserInfo = _context.StackHolerDetails
                                                        .Where(stInfo => stInfo.CompanyId == companyId)
                                                        .Select(stInfo => new MainDto
                                                        {
                                                            SHName = stInfo.ShnameForCompany,
                                                            SHImage = stInfo.Shimage,
                                                            SHJob = stInfo.Shjob,
                                                            CompanyID = stInfo.CompanyId,
                                                            SliceAssets = stInfo.StackHoler.SliceAssets.Select(asset => asset.AssetSlice ).Sum()
                                                        })
                                                        .ToList();
                return MainUserInfo;
            }
            else if (role == 3)
            {
                return null;
            }
            else return null;
        }
    }

    public interface IStackHolderRepository
    {
        IEnumerable<MainDto> GetUserByCompany(string companyId, int role);
        UserLoginDto GetUserInfo(string email);   
    }
}
