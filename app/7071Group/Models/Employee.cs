using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _7071Group.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; } // PK 
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? JobTitle { get; set; }
        public string? EmploymentType { get; set; } 
        public decimal SalaryRate { get; set; } 
        public int? ReportsTo { get; set; } // FK
    }
}
