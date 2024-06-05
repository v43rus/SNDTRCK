using System.ComponentModel.DataAnnotations;

namespace SNDTRCK.Models
{
    public class OrderViewModel
    {
        [Required] 
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }

        [Required] 
        public string Address { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{5}$")] //Endast 5 tecken till�ts
        public string PostalCode { get; set; }
        
        [Required] 
        public string City { get; set; }

        [Required] 
        [Phone]
        [RegularExpression(@"^\+?\d+$")] //Telefonnummret f�r bara inneh�lla siffror, men kan b�rja med ett + f�r riktnummer
        public string PhoneNumber { get; set; }
        
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }
    }
}
