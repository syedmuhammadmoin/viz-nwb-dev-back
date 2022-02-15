﻿using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BusinessPartner : BaseEntity<int>
    {
        public string BusinessPartnerType { get; private set; }
        [MaxLength(50)]
        public string Entity { get; private set; }
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(20)]
        public string CNIC { get; private set; }
        [MaxLength(200)]
        public string Address { get; private set; }
        [MaxLength(50)]
        public string Country { get; private set; }
        [MaxLength(50)]
        public string State { get; private set; }
        [MaxLength(50)]
        public string City { get; private set; }
        [MaxLength(20)]
        public string Phone { get; private set; }
        [MaxLength(20)]
        public string Mobile { get; private set; }
        [MaxLength(50)]
        public string Email { get; private set; }
        [MaxLength(50)]
        public string Website { get; private set; }
        [MaxLength(50)]
        public string IncomeTaxId { get; private set; }
        [MaxLength(50)]
        public string SalesTaxId { get; private set; }
        [MaxLength(50)]
        public string BankAccountTitle { get; private set; }
        [MaxLength(30)]
        public string BankAccountNumber { get; private set; }
        public Guid AccountReceivableId { get; private set; }
        [ForeignKey("AccountReceivableId")]
        public Level4 AccountReceivable { get; private set; }
        public Guid AccountPayableId { get; private set; }
        [ForeignKey("AccountPayableId")]
        public Level4 AccountPayable { get; private set; }

        public BusinessPartner(BusinessPartner businessPartner)
        {
            Entity = businessPartner.Entity;
            Name = businessPartner.Name;
            CNIC = businessPartner.CNIC;
            Address = businessPartner.Address;
            Country = businessPartner.Country;
            State = businessPartner.State;
            City = businessPartner.City;
            Phone = businessPartner.Phone;
            Mobile = businessPartner.Mobile;
            Email = businessPartner.Email;
            Website = businessPartner.Website;
            IncomeTaxId = businessPartner.IncomeTaxId;
            SalesTaxId = businessPartner.SalesTaxId;
            BankAccountTitle = businessPartner.BankAccountTitle;
            BankAccountNumber = businessPartner.BankAccountNumber;
            AccountReceivableId = businessPartner.AccountReceivableId;
            AccountReceivable = businessPartner.AccountReceivable;
            AccountPayableId = businessPartner.AccountPayableId;
            AccountPayable = businessPartner.AccountPayable;
        }
        protected BusinessPartner()
        {

        }
    }
}
