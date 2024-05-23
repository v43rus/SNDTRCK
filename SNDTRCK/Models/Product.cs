using System;
using System.Collections.Generic;

namespace SNDTRCK.Models;

public partial class Product
{
	public int ProductId { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string RecordLabel { get; set; }
	public int ReleaseYear { get; set; }

	public int DiscogId { get; set; }

	public string Genre { get; set; }

	public decimal Price { get; set; }

	public string CoverImageLink { get; set; }

	public string Artist { get; set; }
}
