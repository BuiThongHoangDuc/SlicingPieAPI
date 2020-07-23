using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _company;
        private readonly IStakeHolderRepository _stakeHolder;
        private readonly ISliceAssetRepository _sliceRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITypeAssetRepo _typeAssetRepo;
        private readonly ITypeAssetCompanyRepo _typeAssetCompanyRepo;
        private readonly ITermProjectCompanyRepo _termProjectRepo;
        public CompanyService(ICompanyRepository company,
                                IStakeHolderRepository stakeHolder,
                                ISliceAssetRepository sliceRepository,
                                IProjectRepository projectRepository,
                                ITypeAssetRepo typeAssetRepo,
                                ITypeAssetCompanyRepo typeAssetCompanyRepo,
                                ITermProjectCompanyRepo termProjectRepo)
        {
            _company = company;
            _stakeHolder = stakeHolder;
            _sliceRepository = sliceRepository;
            _projectRepository = projectRepository;
            _typeAssetRepo = typeAssetRepo;
            _typeAssetCompanyRepo = typeAssetCompanyRepo;
            _termProjectRepo = termProjectRepo;
        }

        public List<object> getListCompany(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected)
        {
            var companies = _company.Search(name);
            companies = _company.Sort(companies, sort_Type);
            companies = _company.Paging(companies, page_Index, itemPerPage);
            List<Object> list = _company.Filter(companies, field_Selected);
            return list;
        }

        public async Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID)
        {
            var info = await _stakeHolder.getListShByCompany(companyID);
            return info;
        }

        public async Task<CompanyDetailDto> getDetailCompany(string companyID)
        {
            var companyDetail = await _company.getDetailCompany(companyID);
            return companyDetail;
        }

        public async Task<SHLoadMainDto> getSHByCompany(string companyId, string shId)
        {
            return await _stakeHolder.getShByIDCompany(companyId, shId);
        }

        public async Task<string> updateCompany(string id, CompanyDetailDto company)
        {
            return await _company.UpdateCompany(id, company);
        }

        public async Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company)
        {
            string lastCompanyID = _company.getLastIDCompany();
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(lastCompanyID);

            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;
            int numberCompany = Int32.Parse(numberPart) + 1;
            company.CompanyId = alphaPart + numberCompany;

            if (company.NonCashMultiplier == 0) company.NonCashMultiplier = 2;
            if (company.CashMultiplier == 0) company.CashMultiplier = 4;
            if (company.CashPerSlice == null) company.CashPerSlice = 500000;
            try
            {
                var companyInfo = await _company.CreateCompany(company);
                var listType = await _typeAssetRepo.getListTypeAsset().ToListAsync();
                foreach (var id in listType)
                {
                    await _typeAssetCompanyRepo.CreateAssetCompany(id, companyInfo.CompanyId);
                }
                return companyInfo;
            }
            catch (DbUpdateException)
            {
                throw;
            }



        }
        public bool deleteCompany(string id)
        {
            return _company.deleteCompany(id);
        }

        public IQueryable<ProjectDto> getListProject(string companyId)
        {
            return _projectRepository.getProjectList(companyId);
        }

        public async Task<bool> AddProjectSV(String companyID, ProjectDto project)
        {
            var lastid = _projectRepository.getProjectLast(companyID).Result.ToList().Count;
            if (lastid == 0)
            {
                project.ProjectId = companyID + " PJ1";
            }
            else
            {

                project.ProjectId = companyID + " PJ"+(lastid+1);
            }

            return await _projectRepository.AddProject(project);
        }

        public Task<string> udpateProjectSV(string id, ProjectDto project)
        {
            return _projectRepository.udpateProject(project, id);
        }

        public bool deleteProjectSV(string projectid)
        {
            return _projectRepository.deleteProject(projectid);
        }

        public Task<IEnumerable<TypeAssetDto>> GetListTypeAssetByCompanyIDSV(string companyID)
        {
            return _typeAssetCompanyRepo.GetListTypeAssetByCompanyID(companyID);
        }

        public Task<string> GetNameStakeHolderSV(string userID, string companyID)
        {
            return _stakeHolder.GetNameStakeHolder(userID, companyID);
        }

        public Task<IEnumerable<ProjectDto>> getTermProjectCompanySV(int termID)
        {
            return _termProjectRepo.getTermProjectCompany(termID);
        }

        public Task<IEnumerable<TermDto>> GetListTermCompanySV(string companyID)
        {
            return _termProjectRepo.GetListTermCompany(companyID);
        }

        public async Task<bool> addTermCompanySV(AddTermDto term)
        {
            int length = _termProjectRepo.GetListTermCompany(term.CompanyId).Result.ToList().Count;
            if (length == 0)
            {
                term.TermName = "Term 1";
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                term.TermTimeFrom = firstDayOfMonth;
                term.TermTimeTo = lastDayOfMonth;
            }
            else
            {
                term.TermName = "Term "+(length+1);
                DateTime Lastest = await _termProjectRepo.GetLatestTimeto(term.CompanyId);
                DateTime date = Lastest.AddDays(1);
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                term.TermTimeFrom = firstDayOfMonth;
                term.TermTimeTo = lastDayOfMonth;
            }

            return await _termProjectRepo.addTermCompany(term);
        }

        public async Task<bool> AddProjectToTermSV(int termID, string projectID)
        {
            return await _termProjectRepo.AddProjectToTerm(termID, projectID);
        }

        public IQueryable<string> GetChartCompanySV(string companyid)
        {
            return _company.GetChartCompany(companyid);
        }

        public Task<bool> UpdateDoneTermSV(int termID)
        {
            return _termProjectRepo.UpdateDoneTerm(termID);
        }
    }

    public interface ICompanyService
    {
        Task<IEnumerable<SHLoadMainDto>> getListSHComapny(string companyID);
        Task<SHLoadMainDto> getSHByCompany(string companyId, string shId);
        Task<CompanyDetailDto> getDetailCompany(string companyID);
        List<Object> getListCompany(string name, string sort_Type, int page_Index, int itemPerPage, string field_Selected);
        Task<string> updateCompany(string id, CompanyDetailDto company);
        Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company);
        bool deleteCompany(string id);
        IQueryable<ProjectDto> getListProject(string companyId);
        Task<bool> AddProjectSV(String companyID, ProjectDto project);
        Task<String> udpateProjectSV(String id, ProjectDto project);
        bool deleteProjectSV(string projectid);
        Task<IEnumerable<TypeAssetDto>> GetListTypeAssetByCompanyIDSV(String companyID);
        Task<String> GetNameStakeHolderSV(String userID, String companyID);
        Task<IEnumerable<ProjectDto>> getTermProjectCompanySV(int termID);
        Task<IEnumerable<TermDto>> GetListTermCompanySV(String companyID);
        Task<bool> addTermCompanySV(AddTermDto term);
        Task<bool> AddProjectToTermSV(int termID, string projectID);
        IQueryable<string> GetChartCompanySV(string companyid);
        Task<bool> UpdateDoneTermSV(int termID);

    }
}
