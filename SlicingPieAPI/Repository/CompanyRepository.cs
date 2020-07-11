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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly SWDSlicingPieContext _context;
        public CompanyRepository(SWDSlicingPieContext context)
        {
            _context = context;
        }

        public List<object> Filter(IQueryable<Company> companies, string selectedField)
        {
            var list_query = companies.ToList();

            List<Object> list_return = new List<Object>();
            SupportSelectField supportSelectField = new SupportSelectField();
            foreach (var item in list_query)
            {
                var temp = supportSelectField.getByField(item, selectedField);
                list_return.Add(temp);
            }
            return list_return;
        }

        public async Task<String> GetCompany(string userID)
        {
            string company = await _context.StakeHolders
                                            .Where(sh => sh.AccountId == userID)
                                            .Select(sh => sh.CompanyId).FirstOrDefaultAsync();
            return company;
        }


        public IQueryable<Company> Paging(IQueryable<Company> companies, int pageIndex, int itemPerPage)
        {
            if (pageIndex != -1)
            {
                companies = companies.Skip(pageIndex * itemPerPage).Take(itemPerPage);
            }
            return companies;
        }

        public IQueryable<Company> Search(string search)
        {
            IQueryable<Company> companies = _context.Companies.Where(q => q.CompanyName.Contains(search) && q.Status == Status.ACTIVE);
            return companies;
        }

        public IQueryable<Company> Sort(IQueryable<Company> companies, string typeOfSort)
        {
            switch (typeOfSort)
            {
                case "asc": companies = companies.OrderBy(p => p.CompanyName); break;
                case "des": companies = companies.OrderByDescending(p => p.CompanyName); break;
            }
            return companies;
        }

        public async Task<CompanyDetailDto> getDetailCompany(string companyId) {
            var companyInfo = await _context.Companies
                                        .Where(company => company.CompanyId == companyId)
                                        .Select(company => new CompanyDetailDto
                                        {
                                            CompanyId = company.CompanyId,
                                            CompanyName = company.CompanyName,
                                            ComapnyIcon = company.ComapnyIcon,
                                            CashMultiplier = company.CashMultiplier,
                                            NonCashMultiplier = company.NonCashMultiplier,
                                            CashPerSlice = company.CashPerSlice,
                                        }).FirstOrDefaultAsync();
            return companyInfo;
        }

        public async Task<string> UpdateCompany(string id, CompanyDetailDto company)
        {

            Company dto = await _context.Companies.FindAsync(id);
            dto.CompanyName = company.CompanyName;
            dto.ComapnyIcon = company.ComapnyIcon;
            dto.NonCashMultiplier = company.NonCashMultiplier;
            dto.CashMultiplier = company.CashMultiplier;
            dto.CashPerSlice = company.CashPerSlice;

            _context.Entry(dto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return dto.CompanyId;
        }

        public async Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company)
        {
            Company companyModel = new Company();
            companyModel.CompanyId = company.CompanyId;
            companyModel.CompanyName = company.CompanyName;
            companyModel.ComapnyIcon = company.ComapnyIcon;
            companyModel.NonCashMultiplier = company.NonCashMultiplier;
            companyModel.CashMultiplier = company.CashMultiplier;
            companyModel.CashPerSlice = company.CashPerSlice;
            companyModel.Status = Status.ACTIVE;

            _context.Companies.Add(companyModel);
            try
            {
                await _context.SaveChangesAsync();
                return company;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public string getLastIDCompany()
        {
            return _context.Companies.OrderByDescending(company => company.CompanyId).Select(company => company.CompanyId).FirstOrDefault();
        }

        public bool deleteCompany(string id)
        {
            var company = _context.Companies.Find(id);
            if (company == null) return false;
            else
            {
                company.Status = Status.INACTIVE;
                _context.SaveChanges();
                return true;
            }
        }

        public async Task<double> GetMoneyPerSlice(string companyID)
        {
            var cashperslice = await _context.Companies.Where(cp => cp.CompanyId == companyID).Select(cp => cp.CashPerSlice).FirstOrDefaultAsync();
            return (double)cashperslice;
        }

        public async Task<int> GetNonCashMP(string companyID)
        {
            var nonCashMP = await _context.Companies.Where(cp => cp.CompanyId == companyID).Select(cp => cp.NonCashMultiplier).FirstOrDefaultAsync();
            return nonCashMP;
        }

        public async Task<int> GetCashMP(string companyID)
        {
            var cashMP = await _context.Companies.Where(cp => cp.CompanyId == companyID).Select(cp => cp.CashMultiplier).FirstOrDefaultAsync();
            return cashMP;
        }
    }

    public interface ICompanyRepository
    {
        Task<string> GetCompany(string email);
        Task<CompanyDetailDto> getDetailCompany(string companyId);
        IQueryable<Company> Search(string search);

        IQueryable<Company> Paging(IQueryable<Company> companies, int pageIndex, int itemPerPage);

        IQueryable<Company> Sort(IQueryable<Company> companies, string typeOfSort);

        List<Object> Filter(IQueryable<Company> companies, string selectedField);

        Task<string> UpdateCompany(string companyId,CompanyDetailDto company);

        Task<CompanyDetailDto> CreateCompany(CompanyDetailDto company);
        string getLastIDCompany();
        bool deleteCompany(string id);
        Task<double> GetMoneyPerSlice(String companyID);
        Task<int> GetNonCashMP(String companyID);
        Task<int> GetCashMP(String companyID);
    }
}
