namespace _7071Group.Models
{
    public class Payroll
    {
        public int PayrollID { get; set; } // PK
        public int EmployeeID { get; set; } // FK 
        public decimal BaseSalary { get; set; }
        public decimal OvertimePay { get; set; } 
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; } 
        public DateTime PayDate { get; set; }
    }
}
