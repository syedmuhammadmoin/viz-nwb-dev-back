using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGRNToGoodsReturnNoteReconcileRepository
    {
        Task<GRNToGoodsReturnNoteReconcile> Add(GRNToGoodsReturnNoteReconcile entity);

        IEnumerable<GRNToGoodsReturnNoteReconcile> Find(ISpecification<GRNToGoodsReturnNoteReconcile> specification);

    }
}
