﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<String> GetCompany(string userID)
        {
            string company = await _context.StackHolerDetails
                                            .Where(stDt => stDt.StackHolerId == userID)
                                            .Select(stDt => stDt.CompanyId).FirstOrDefaultAsync();
            return company;
        }
    }

    public interface ICompanyRepository
    {
        Task<string> GetCompany(string email);
    }
}
