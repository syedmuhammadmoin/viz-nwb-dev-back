using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGRNToGoodsReturnNoteLineReconcileRepository
    {
        Task<GRNToGoodsReturnNoteLineReconcile> Add(GRNToGoodsReturnNoteLineReconcile entity);

        IEnumerable<GRNToGoodsReturnNoteLineReconcile> Find(ISpecification<GRNToGoodsReturnNoteLineReconcile> specification);

    }
}
