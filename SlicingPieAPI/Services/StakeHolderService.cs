using SlicingPieAPI.DTOs;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class StakeHolderService : IStakeHolderService
    {
        private readonly IStakeHolderRepository _stakeHolder;
        public StakeHolderService(IStakeHolderRepository stakeHolder)
        {
            _stakeHolder = stakeHolder;
        }

        public async Task<IEnumerable<SHLoadMainDto>> getListSHByCompany(string companyId)
        {
            return await _stakeHolder.getListShByCompany(companyId);
        }

        public async Task<SHLoadMainDto> getSHByCompany(string companyId, string shId)
        {
            return await _stakeHolder.getShByIDCompany(companyId, shId);
        }

        public async Task<StakeHolderDto> getStakeHolderLoginInoByID(string id)
        {
            return await _stakeHolder.getSHLoginInfoByID(id);
        }

        public async Task<string> getStakeHolderCompanyID(string id)
        {
            return await _stakeHolder.getStakeHolderCompany(id);
        }

        
    }

    public interface IStakeHolderService
    {
        Task<string> getStakeHolderCompanyID(string id);
        Task<StakeHolderDto> getStakeHolderLoginInoByID(string id);
        Task<IEnumerable<SHLoadMainDto>> getListSHByCompany(string companyId);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);
    }
}
