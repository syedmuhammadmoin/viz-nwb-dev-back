using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Department : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }

        protected Department()
        {

        }
    }
}
