namespace _7071Group.Models
{
    public class Service
    { 
        public int ServiceID { get; set; } // PK 
        public string? ServiceName { get; set; } 
        public decimal Rate { get; set; } 
        public bool RequiresCertification { get; set; }
    }
}
