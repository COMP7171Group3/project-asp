namespace _7071Group.Models
{
    public class DamageReport
    { 
        public int ReportID { get; set; } // PK 
        public int AssetID { get; set; } // FK 
        public string? Description { get; set; }
        public decimal RepairCost { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
