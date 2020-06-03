using System;
using System.Collections.Generic;

namespace SlicingPieAPI.Models
{
    public partial class Company
    {
        public Company()
        {
            Projects = new HashSet<Project>();
            StackHolerDetails = new HashSet<StackHolerDetail>();
            TermOfCompanies = new HashSet<TermOfCompany>();
        }

        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ComapnyIcon { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<StackHolerDetail> StackHolerDetails { get; set; }
        public virtual ICollection<TermOfCompany> TermOfCompanies { get; set; }
    }
}
