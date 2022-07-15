using Application.Contracts.Filters;
using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class GoodsReturnNoteSpecs : BaseSpecification<GoodsReturnNoteMaster>
    {
        public GoodsReturnNoteSpecs(List<DateTime?> docDate,
            List<DocumentStatus?> states, TransactionFormFilter filter, bool isTotalRecord)
            : base(x => (docDate.Count() > 0 ? docDate.Contains(x.ReturnDate) : true)
            && x.DocNo.Contains(filter.DocNo != null ? filter.DocNo : "")
            && x.Vendor.Name.Contains(filter.BusinessPartner != null ? filter.BusinessPartner : "")
            && (states.Count() > 0 ? states.Contains(x.Status.State) : true))
        {
            if (!isTotalRecord)
            {
                var validFilter = new PaginationFilter(filter.PageStart, filter.PageEnd);
                ApplyPaging(validFilter.PageStart, validFilter.PageEnd - validFilter.PageStart);
                ApplyOrderByDescending(i => i.Id);
                AddInclude(i => i.Campus);
                AddInclude(i => i.Status);
                AddInclude(i => i.Vendor);
                AddInclude(i => i.GRN);
                AddInclude("GoodsReturnNoteLines.Item");
                AddInclude("GoodsReturnNoteLines.Warehouse");
            }
        }
            public GoodsReturnNoteSpecs(bool forEdit)
            {
                if (forEdit)
                {
                    AddInclude(i => i.GoodsReturnNoteLines);
                    AddInclude(i => i.Status);
                }
                else
                {
                    AddInclude(i => i.Campus);
                    AddInclude(i => i.Status);
                    AddInclude(i => i.Vendor);
                    AddInclude(i => i.GRN);
                    AddInclude("GoodsReturnNoteLines.Item");
                    AddInclude("GoodsReturnNoteLines.Warehouse");
                }
            }
            public GoodsReturnNoteSpecs() : base(e => (e.Status.State != DocumentStatus.Unpaid && e.Status.State != DocumentStatus.Partial && e.Status.State != DocumentStatus.Paid && e.Status.State != DocumentStatus.Draft && e.Status.State != DocumentStatus.Cancelled))
        {
                AddInclude(i => i.Status);
            }
            public GoodsReturnNoteSpecs(int id) : base(x => x.Id == id)
        {
                AddInclude(i => i.GoodsReturnNoteLines);
            }
        }
    }
