using System.ComponentModel.DataAnnotations;

namespace SNDTRCK.Models
{
    public class OrderViewModel
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string PostalCode { get; set; }
        [Required] public string City { get; set; }
        [Required] [Phone] public string PhoneNumber { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
    }
}
