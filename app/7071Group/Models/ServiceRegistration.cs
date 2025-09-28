namespace _7071Group.Models
{
    public class ServiceRegistration
    { 
        public int RegistrationID { get; set; } // PK  
        public int ClientID { get; set; } // FK 
        public int ServiceID { get; set; } // FK 
        public DateTime RegistrationDate { get; set; }
    }
}
