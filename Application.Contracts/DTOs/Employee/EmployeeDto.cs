using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Employee
{
    public class EmployeeDto
    {
        public string Name { get; private set; }
        public string FatherName { get; private set; }
        public string CNIC { get; private set; }
        public string Domicile { get; private set; }
        public string Contact { get; private set; }
        public string Religion { get; private set; }
        public string Nationality { get; private set; }
        public string Maritalstatus { get; private set; }
        public string Gender { get; private set; }
        public string PlaceofBirth { get; private set; }
        public int DesignationId { get; private set; }
        public string DesignationName { get; private set; }
        public int DepartmentId { get; private set; }
        public string DepartmentName { get; private set; }
        public string Address { get; private set; }
        public DateTime DateofJoining { get; private set; }
        public DateTime DateofRetirment { get; private set; }
        public DateTime DateofBirth { get; private set; }
        public int EarnedLeaves { get; private set; }
        public int CasualLeaves { get; private set; }
        public string Status { get; private set; }
        public string Role { get; private set; }
        public string Faculty { get; private set; }
        public string DutyShift { get; private set; }
    }
}
