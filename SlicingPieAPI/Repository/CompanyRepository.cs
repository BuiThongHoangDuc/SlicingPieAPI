using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlicingPieAPI.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly SWD_SlicingPieContext _context;
        public CompanyRepository(SWD_SlicingPieContext context)
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

        public async Task<IEnumerable<SHLoadMainDto>> getListStakeHolder(string companyID)
        {
            return null;
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
            IQueryable<Company> companies = _context.Companies.Where(q => q.CompanyName.Contains(search));
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
    }

    public interface ICompanyRepository
    {
        Task<string> GetCompany(string email);
        Task<IEnumerable<SHLoadMainDto>> getListStakeHolder(string companyID);
        IQueryable<Company> Search(string search);

        IQueryable<Company> Paging(IQueryable<Company> companies, int pageIndex, int itemPerPage);

        IQueryable<Company> Sort(IQueryable<Company> companies, string typeOfSort);

        List<Object> Filter(IQueryable<Company> companies, string selectedField);
    }
}
