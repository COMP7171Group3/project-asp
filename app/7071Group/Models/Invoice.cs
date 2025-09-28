namespace _7071Group.Models
{
    public class Invoice
    { 
        public int InvoiceID { get; set; } // PK 
        public int ClientID { get; set; } // FK  
        public int ServiceID { get; set; } // FK  
        public decimal TotalAmount { get; set; } 
        public bool IsPaid { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
