using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class AdmissionApplicationSpecs : BaseSpecification<AdmissionApplication>
    {
        public AdmissionApplicationSpecs(TransactionFormFilter filter, bool isTotalRecord)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
            }
            AddInclude(i => i.Applicant);
            AddInclude(i => i.Batch);
            AddInclude(i => i.Program);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Shift);
        }

        public AdmissionApplicationSpecs(TransactionFormFilter filter, bool isTotalRecord, int applicantId)
            : base (i => i.ApplicantId == applicantId)
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
            }
            AddInclude(i => i.Applicant);
            AddInclude(i => i.Batch);
            AddInclude(i => i.Program);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Shift);
        }

        public AdmissionApplicationSpecs()
        {
            AddInclude(i => i.Applicant);
            AddInclude(i => i.Batch);
            AddInclude(i => i.Program);
            AddInclude(i => i.Campus);
            AddInclude(i => i.Shift);
            AddInclude(i => i.AdmissionCriteria);
        }

        public AdmissionApplicationSpecs(int applicantId, bool forEdit) : base(i => i.ApplicantId == applicantId)
        {
            if (!forEdit)
            {
                AddInclude(i => i.Applicant);
                AddInclude(i => i.Batch);
                AddInclude(i => i.Program);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Shift);
                AddInclude(i => i.AdmissionCriteria);
            }
        }
    }
}
