using Application.Contracts.Response;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Helper
{
    public static class ReceivableAndPayable
    {
    
        public static bool Validate(Guid id)
        {
            if (id == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
                               || id == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00")
                               )
            {
                return false; // Account Invalid
            }
            else
            {
                return true; // Account valid
            }
        }

        public static bool ValidateBankAccount(Guid id)
        {
            if( id == new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"))
            {
                return true; // Account Valid
            }
            else
            {
                return false; // Account Invalid
            }
        }
        public static bool ValidateReceivable(Guid id)
        {
            if( 
             id == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                 || id == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                 || id == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                 || id == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                 || id == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                 || id == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00"))
            {
                return true; // Account Receivable
            }
            else
            {
                return false; // Account Invalid
            }
        }
        public static bool ValidatePayable(Guid id)
        {
            if (
             id == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
             {
                return true; // Account Payable
            }
            else
            {
                return false;// Account Invalid
            }
        }
    }
}
