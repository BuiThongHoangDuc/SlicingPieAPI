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

        public List<Object> getStakeHolder(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected)
        {
            var stakeHolder = _stakeHolder.Search(name);
            stakeHolder = _stakeHolder.Sort(stakeHolder, sort_Type);
            stakeHolder = _stakeHolder.Paging(stakeHolder, page_Index, itemPerPage);
            List<Object> list = _stakeHolder.Filter(stakeHolder, field_Selected);
            return list;
        }

        public async Task<bool> AddStakeHolderSV(AddStakeHolderDto addModel)
        {
            return await _stakeHolder.AddStakeHolder(addModel);
        }

        public IQueryable<AddStakeHolderDto> GetShSV(string companyID, string accountID)
        {
            return _stakeHolder.GetSh(companyID, accountID);
        }

        public Task<bool> UpdateShByIDSV(AddStakeHolderDto editModel)
        {
            return _stakeHolder.UpdateShByID(editModel);
        }

        public Task<bool> DeleteShByID(string companyID, string accountID)
        {
            return _stakeHolder.DeleteShByID(companyID, accountID);
        }

        public Task<IEnumerable<SHLoadMainDto>> getListShByCompanyInactive(string companyId)
        {
            return _stakeHolder.getListShByCompanyInactive(companyId);
        }

        public IQueryable<CompanyListStakeholderDto> GetListCompanyStakeholder(string companyid, string shID)
        {
            return _stakeHolder.GetListCompanyStakeholder(companyid, shID);
        }
    }

    public interface IStakeHolderService
    {
        Task<IEnumerable<SHLoadMainDto>> getListShByCompanyInactive(string companyId);
        Task<string> getStakeHolderCompanyID(string id);
        Task<StakeHolderDto> getStakeHolderLoginInoByID(string id);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);

        List<Object> getStakeHolder(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected);
        Task<bool> AddStakeHolderSV(AddStakeHolderDto addModel);
        IQueryable<AddStakeHolderDto> GetShSV(String companyID, String accountID);
        Task<bool> UpdateShByIDSV(AddStakeHolderDto editModel);
        Task<bool> DeleteShByID(String companyID, String accountID);
        IQueryable<CompanyListStakeholderDto> GetListCompanyStakeholder(string companyid, string shID);

    }
}
