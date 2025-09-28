namespace _7071Group.Models
{
    public class ServiceAssignment
    {
        public int AssignedID { get; set; } // PK
        public int EmployeeID { get; set; } // FK
        public int ServiceID { get; set; } // FK
        public DateTime ScheduledDate { get; set; }
    }
}
