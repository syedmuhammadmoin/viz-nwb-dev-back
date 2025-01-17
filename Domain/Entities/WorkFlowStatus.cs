﻿using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowStatus : BaseEntity<int>
    {
        [MaxLength(500)]
        public string Status { get; private set; }
        public DocumentStatus State { get; private set; }
        public StatusType Type { get; private set; }

        protected WorkFlowStatus()
        {

        }

        public WorkFlowStatus(int id, string status, DocumentStatus state, StatusType type)
        {
            Id = id;
            Status = status;
            State = state;
            Type = type;
        }
    }
}
