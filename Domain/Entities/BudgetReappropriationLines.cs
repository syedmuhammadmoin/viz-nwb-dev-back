using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BudgetReappropriationLines : BaseEntity<int>
    {
        public Guid Level4Id { get; private set; }
        [ForeignKey("Level4Id")]
        public Level4 Level4 { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        [MaxLength(500)]
        public string Description { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeletionAmount { get; set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public BudgetReappropriationMaster BudgetReappropriationMaster { get; private set; }

        protected BudgetReappropriationLines()
        {
        }
 
    }
}