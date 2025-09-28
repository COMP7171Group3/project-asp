namespace _7071Group.Models
{
    public class Renter
    {
        public int RenterID { get; set; } // PK 
        public string? Name { get; set; }
        public string? EmergencyContact { get; set; } 
        public string? FamilyDoctor { get; set; }
    }
}
