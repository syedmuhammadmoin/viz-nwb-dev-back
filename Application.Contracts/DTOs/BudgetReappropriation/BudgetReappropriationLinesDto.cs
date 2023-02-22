using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class BudgetReappropriationLinesDto
    {
        public int  Id { get; set; }
        public Guid Level4Id { get; set; }
        public string Level4 { get; set; }
        public int CampusId { get; set; }
        public string Campus { get; set; }
        public string Description { get; set; }
        public decimal AdditionAmount { get; set; }
        public decimal DeletionAmount { get; set; }
        public int MasterId { get; set; }
       
    }
}
