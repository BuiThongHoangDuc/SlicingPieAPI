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

        public List<Object> getStakeHolder(string name, int page_Index, int itemPerPage ,string sort_Type, string field_Selected)
        {
            var stakeHolder = _stakeHolder.Search(name);
            stakeHolder = _stakeHolder.Paging(stakeHolder, page_Index, itemPerPage);
            stakeHolder = _stakeHolder.Sort(stakeHolder, sort_Type);
            List<Object> list = _stakeHolder.Filter(stakeHolder, field_Selected);
            return list;
        }
    }

    public interface IStakeHolderService
    {
        Task<string> getStakeHolderCompanyID(string id);
        Task<StakeHolderDto> getStakeHolderLoginInoByID(string id);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);

        List<Object> getStakeHolder(string name, int page_Index, int itemPerPage, string sort_Type, string field_Selected);
    }
}
