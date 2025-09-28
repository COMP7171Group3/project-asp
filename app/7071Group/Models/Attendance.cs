namespace _7071Group.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; } // PK
        public int EmployeeID { get; set; } // FK
        public int ShiftID { get; set; } // FK
        public bool IsHoliday { get; set; }
        public bool IsVacation { get; set; }
        public bool IsOnCall { get; set; }
    }
}
