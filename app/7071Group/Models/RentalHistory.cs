namespace _7071Group.Models
{
    public class RentalHistory
    { 
        public int HistoryID { get; set; } // PK 
        public int RenterID { get; set; } // FK
        public int AssetID { get; set; } // FK 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public decimal RentAmount { get; set; }
    }
}
