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
    public class RequestLines : BaseEntity<int>
    {
        [MaxLength(500)]
        public string ItemDescription { get; private set; }
        public int ItemQuantity { get; private set; }
        public int MasterId { get; private set; }
        [ForeignKey("MasterId")]
        public RequestMaster RequestMaster { get; private set; }
        protected RequestLines()
        {
        }
    }
}
