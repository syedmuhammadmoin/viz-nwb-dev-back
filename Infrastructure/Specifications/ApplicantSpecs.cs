using Application.Contracts.Filters;
using Domain.Entities;

namespace Infrastructure.Specifications
{
    public class ApplicantSpecs : BaseSpecification<Applicant>
    {
        public ApplicantSpecs(TransactionFormFilter filter, bool isTotalRecord)
            : base(c => c.Name.Contains(filter.Name != null ? filter.Name : ""))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.PlaceOfBirth);
                AddInclude(i => i.Domicile);
                AddInclude(i => i.Nationality);
            }
        }

        public ApplicantSpecs(bool forEdit)
        {
            if (forEdit)
            {
                AddInclude(i => i.Qualifications);
                AddInclude(i => i.Relatives);
            }
            else
            {
                AddInclude("Qualifications.Qualification");
                AddInclude("Qualifications.Subject");
                AddInclude(i => i.Relatives);
                AddInclude(i => i.PlaceOfBirth);
                AddInclude(i => i.Domicile);
                AddInclude(i => i.Nationality);
            }
        }

        public ApplicantSpecs(string emailOrCnic, int option) // 1 for email, 2 for cnic
            :base(i => option == 1 ? i.Email == emailOrCnic.Trim()
            : i.CNIC == emailOrCnic.Trim())
        { 
        }
        
        public ApplicantSpecs(string userId) : base (i => i.UserId == userId)
        {
        }
    }
}
