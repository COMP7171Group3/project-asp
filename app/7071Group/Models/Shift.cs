namespace _7071Group.Models
{
    public class Shift
    {
        public int ShiftID { get; set; } // PK
        public int EmployeeID { get; set; } // FK
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnCall { get; set; }
    }
}
