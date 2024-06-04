using System;
using System.Collections.Generic;

namespace SNDTRCK.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal OrderSum { get; set; }

    public DateTime OrderDate { get; set; }

    public string? UserId { get; set; }

    public string? OrderStatus { get; set; }
}
