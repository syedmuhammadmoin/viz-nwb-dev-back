﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RequestMaster : BaseEntity<int>
    {
        [MaxLength(30)]
        public string DocNo { get; private set; }
        public DateTime RequestDate { get; private set; }
        public int EmployeeId { get; private set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; private set; }
        public int CampusId { get; private set; }
        [ForeignKey("CampusId")]
        public Campus Campus { get; private set; }
        public int StatusId { get; private set; }
        [ForeignKey("StatusId")]
        public WorkFlowStatus Status { get; private set; }
        public virtual List<RequestLines> RequestLines { get; private set; }

        protected RequestMaster()
        {

        }

        public void SetStatus(int statusId)
        {
            StatusId = statusId;
        }

        public void CreateDocNo()
        {
            //Creating doc no..
            DocNo = "REQUEST-" + String.Format("{0:000}", Id);
        }
    }
}
