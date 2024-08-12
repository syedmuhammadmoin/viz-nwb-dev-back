using Domain.Base;
using Domain.Constants;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Level4 : BaseEntity<string>, IMustHaveTenant
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public AccountType AccountType { get; set; }
        public string Level3_id { get; set; }
        [ForeignKey("Level3_id")]
        public Level3 Level3 { get; private set; }
        public string Level1_id { get; set; }
        [ForeignKey("Level1_id")]
        public Level1 Level1 { get; private set; }
        public int OrganizationId { get; set; }

        public Level4()
        {
        }
        public Level4(string id, string name, string level3_id, string level1_id, int orgId)
        {
            Id = id;
            Name = name;
            Level3_id = level3_id;
            Level1_id = level1_id;
            OrganizationId = orgId;
            AccountType = AccountType.SystemDefined;
        }
        public Level4(string name, string accountCode, string level3_id, string level1_id, int orgId, bool NeedToFixed)
        {
            Name = name;
            Code = accountCode;
            Level3_id = level3_id;
            Level1_id = level1_id;
            AccountType = AccountType.SystemDefined;
        }
        public Level4(string name, string level3_id)
        {
            Name = name;
            Level3_id = level3_id;
            AccountType = AccountType.UserDefined;
        }
        public void SetAccountName(string name, string accountCode)
        {
            Name = name;
            Code = accountCode;
        }

        public void SetLevel1Id(string level1Id)
        {
            Level1_id = level1Id;
        }
    }
}
