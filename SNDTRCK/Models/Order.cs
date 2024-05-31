using System;
using System.Collections.Generic;

namespace SNDTRCK.Models;

public partial class Order
{
	public int OrderId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string PostalCode { get; set; }
	public string City { get; set; }	
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public int Quantity { get; set; }
	public decimal OrderSum { get; set; }
	public DateTime OrderDate { get; set; }
}
