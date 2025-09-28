namespace _7071Group.Models
{
    public class Asset
    { 
        public int AssetID { get; set; } // PK 
        public string? AssetType { get; set; }
        public string? Location { get; set; }
        public decimal MonthlyRent { get; set; }
    }
}
