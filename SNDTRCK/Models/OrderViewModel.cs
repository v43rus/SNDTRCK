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
        [RegularExpression(@"^\d{5}$")] //Endast 5 tecken tillåts
        public string PostalCode { get; set; }
        
        [Required] 
        public string City { get; set; }

        [Required] 
        [Phone]
        [RegularExpression(@"^\+?\d+$")] //Telefonnummret får bara innehålla siffror, men kan börja med ett + för riktnummer
        public string PhoneNumber { get; set; }
        
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }
    }
}
